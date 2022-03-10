using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
	[Header("Set Dynamically")]
    public Rigidbody rigid; // a
	
	private Neighborhood neighborhood;

   // Use this for initialization
   void Awake () {
   neighborhood = GetComponent<Neighborhood>();
   rigid = GetComponent<Rigidbody>(); // a
 
   // Set a random initial position
   pos = Random.insideUnitSphere * Spawner.S.spawnRadius; // b
 
   // Set a random initial velocity
   Vector3 vel = Random.onUnitSphere * Spawner.S.velocity; // c
   rigid.velocity = vel;

   LookAhead(); // d

   // Give the Boid a random color, but make sure it's not too dark // e
   Color randColor = Color.black;
   while ( randColor.r + randColor.g + randColor.b < 1.0f ) {
      randColor = new Color(Random.value, Random.value, Random.value);
   }
   Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>(); //f  
   foreach ( Renderer r in rends )
   {
     r.material.color = randColor;
    }
   TrailRenderer tRend = GetComponent<TrailRenderer>();
    tRend.material.SetColor("_TintColor", randColor);
 }

 void LookAhead() { // d
 // Orients the Boid to look at the direction it's flying
 transform.LookAt(pos + rigid.velocity);
 }

 public Vector3 pos { // b
 get { return transform.position; }
 set { transform.position = value; }
 }
 
 // FixedUpdate is called once per physics update (i.e., 50x/second)
 void FixedUpdate () {
 Vector3 vel = rigid.velocity; // b
 Spawner spn = Spawner.S; // c

// COLLISION AVOIDANCE – Avoid neighbors who are too close
 Vector3 velAvoid = Vector3.zero;
 Vector3 tooClosePos = neighborhood.avgClosePos;
 // If the response is Vector3.zero, then no need to react
 if (tooClosePos != Vector3.zero) {
 velAvoid = pos - tooClosePos;
 velAvoid.Normalize();
 velAvoid *= spn.velocity;
 }

 // VELOCITY MATCHING – Try to match velocity with neighbors
 Vector3 velAlign = neighborhood.avgVel;
 // Only do more if the velAlign is not Vector3.zero
 if (velAlign != Vector3.zero) {
 // We're really interested in direction, so normalize the velocity
 velAlign.Normalize();
 // and then set it to the speed we chose
 velAlign *= spn.velocity;
 }

 // FLOCK CENTERING – Move towards the center of local neighbors
 Vector3 velCenter = neighborhood.avgPos;
 if (velCenter != Vector3.zero) {
 velCenter -= transform.position;
 velCenter.Normalize();
 velCenter *= spn.velocity;
 }

 // ATTRACTION – Move towards the Attractor
 Vector3 delta = Attractor.POS - pos; // d
 // Check whether we're attracted or avoiding the Attractor
 bool attracted = (delta.magnitude > spn.attractPushDist);
 Vector3 velAttract = delta.normalized * spn.velocity; // e

 // Apply all the velocities
 float fdt = Time.fixedDeltaTime;
if (velAvoid != Vector3.zero) {
  vel = Vector3.Lerp(vel, velAvoid, spn.collAvoid*fdt);
 } else {
 if (velAlign != Vector3.zero) {
 vel = Vector3.Lerp(vel, velAlign, spn.velMatching*fdt);
 }
 if (velCenter != Vector3.zero) {
vel = Vector3.Lerp(vel, velAlign, spn.flockCentering*fdt);
 }
 if (velAttract != Vector3.zero) {
 if (attracted) {
 vel = Vector3.Lerp(vel, velAttract, spn.attractPull*fdt);
 } else {
 vel = Vector3.Lerp(vel, -velAttract, spn.attractPush*fdt);
 }
 }
 }

 if (attracted) { // f
 vel = Vector3.Lerp(vel, velAttract, spn.attractPull*fdt);
 } else {
 vel = Vector3.Lerp(vel, -velAttract, spn.attractPush*fdt);
 }

 // Set vel to the velocity set on the Spawner singleton
 vel = vel.normalized * spn.velocity; // g
 // Finally assign this to the Rigidbody
 rigid.velocity = vel;
 // Look in the direction of the new velocity
LookAhead();
 }
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
