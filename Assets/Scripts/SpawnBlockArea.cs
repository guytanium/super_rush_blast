﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlockArea : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnTriggerEnter2D(Collider2D col)
    {
        //all projectile colliding game objects should be tagged "Enemy" or whatever in inspector but that tag must be reflected in the below if conditional
        if (col.gameObject.tag == "Spawn")
        {
            col.gameObject.GetComponent<SpawnPoint>().SpawnDisable();

            //        Destroy(col.gameObject);
            //add an explosion or something
            //destroy the projectile that just caused the trigger collision
            //Destroy(gameObject);
        }
    }

}
