using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull2BoneRotator : MonoBehaviour {

    [Range(250,500)] 
    public float minimumSpeed;
    [Range(500,700)]
    public float maximumSpeed;
    public bool clockwise;
    private float rotationSpeed;

	// Use this for initialization
	void Start () {
        rotationSpeed = Random.Range(minimumSpeed, maximumSpeed);
        if (Random.Range(0,2) == 1)
        {
            
            clockwise = true;
        }
        else
        {
            clockwise = false;
        }
        Debug.Log(clockwise);
		
	}
	
	// Update is called once per frame
	void Update () 
    {

        int rotationMultiplier = 0;

            if(clockwise)
        {
             rotationMultiplier = -1;
        }
        else if (!clockwise)
        {
             rotationMultiplier = 1;
        }
        transform.Rotate(Vector3.forward * Time.deltaTime* (rotationSpeed)*rotationMultiplier);

	}
}
