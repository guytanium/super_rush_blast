﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBasic : MonoBehaviour {


    private Vector3 playerTransform;
    private Vector3 playerDirection;
    private float xDif;
    private float yDif;
    private Rigidbody2D myRB;
    public float speed;


	// Use this for initialization
	void Start () 
    {
        myRB = GetComponent<Rigidbody2D>();
		
	}

//    void Awake () 
//    {
//        myRB.MovePosition(Vector2(0,0);
//            
//    }

	
	// Update is called once per frame
	void FixedUpdate () {
        playerTransform = GameObject.FindWithTag("Player").transform.position;  

        xDif = playerTransform.x - transform.position.x;
        yDif = playerDirection.y - transform.position.y;

        playerDirection = new Vector3(xDif, yDif);

      //  myRB.AddForce(playerDirection.normalized);

//        myRB.AddForce(playerDirection.normalized * speed);
///        myRB.AddForce(playerDirection.normalized - transform.position.normalized);
//        myRB.AddForce(playerTransform.normalized);
        myRB.AddForce(playerTransform.normalized - transform.position.normalized);




		
	}
}
