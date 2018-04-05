using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
	static public Hero	S; // Singleton

	// Ship Movement Settings
	public float	speed = 30;
    //public float	rollMult = -45;
    //public float	pitchMult = 30;
    public float	rollMult = 45;
    public float	pitchMult = 120;
    public float    orientation = 0;

    public float	shieldLevel = 1;

	// Use this for initialization
	void Awake () {
		S = this;
	}

	// Update is called once per frame
	void Update () {
		float xAxis = Input.GetAxis ("Horizontal");
		float yAxis = Input.GetAxis ("Vertical");

		Vector3 pos = transform.position;
		//pos.x += xAxis * speed * Time.deltaTime;
		//pos.y += yAxis * speed * Time.deltaTime;
		pos.x += xAxis * speed * Time.deltaTime;
		pos.z += yAxis * speed * Time.deltaTime;

		transform.position = pos;

        // Move the Ship
        //transform.rotation = Quaternion.Euler(yAxis*pitchMult,xAxis*rollMult,0);
        // If user Presses Spacebare then Rotate

        if ((Input.GetKeyDown(KeyCode.UpArrow) ||
			Input.GetKeyDown(KeyCode.W))) 
           orientation = 2;         

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            if (orientation == 2)
               orientation = 1.5f;
        //if (orientation == 2)
        //    orientation = 2.5f;

        /* else if (Input.GetKeyDown(KeyCode.DownArrow))
            orientation = 4;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            orientation = 1;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            orientation = 3; */

        if (Input.GetKeyDown(KeyCode.R))
           if (orientation == 4)
                orientation = 1;
           else
                orientation++;
        transform.rotation = Quaternion.Euler(90*rollMult,90*orientation,180*pitchMult);
    }
}
