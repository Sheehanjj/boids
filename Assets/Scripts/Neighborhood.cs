﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : MonoBehaviour
{
	[Header("Set Dynamically")]
    public List<Boid> neighbors;
    private SphereCollider coll;
    void Start() { // a
      neighbors = new List<Boid>();
      coll = GetComponent<SphereCollider>();
      coll.radius = Spawner.S.neighborDist/2;
    }
 
 void FixedUpdate() { // b
 if (coll.radius != Spawner.S.neighborDist/2) {
    coll.radius = Spawner.S.neighborDist/2;
  }
  }
 
void OnTriggerEnter(Collider other) { // c
 Boid b = other.GetComponent<Boid>();
 if (b != null) {
 if (neighbors.IndexOf(b) == -1) {
     neighbors.Add(b);
	}
 }
}
 
void OnTriggerExit(Collider other) { // d
  Boid b = other.GetComponent<Boid>();
 if (b != null) {
 if (neighbors.IndexOf(b) != -1) {
	neighbors.Remove(b);
   }
  }
 }
 
 public Vector3 avgPos { // e
 get {
 Vector3 avg = Vector3.zero;
 if (neighbors.Count == 0) return avg;

for (int i=0; i<neighbors.Count; i++) {
avg += neighbors[i].pos;
}
avg /= neighbors.Count;

return avg;
}
}

public Vector3 avgVel { // f
 get {
 Vector3 avg = Vector3.zero;
 if (neighbors.Count == 0) return avg;

for (int i=0; i<neighbors.Count; i++) {
 avg += neighbors[i].rigid.velocity;
 }
avg /= neighbors.Count;

 return avg;
}
 }

 public Vector3 avgClosePos { // g
 get {
 Vector3 avg = Vector3.zero;
 Vector3 delta;
 int nearCount = 0;
 for (int i=0; i<neighbors.Count; i++) {
 delta = neighbors[i].pos - transform.position;
 if (delta.magnitude <= Spawner.S.collDist) {
 avg += neighbors[i].pos;
 nearCount++;
 }
 }
 // If there were no neighbors too close, return Vector3.zero
 if (nearCount == 0) return avg;

 // Otherwise, average their locations
 avg /= nearCount;
 return avg;
 }
 }
}
