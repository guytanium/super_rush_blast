using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class Eye1AI_test : MonoBehaviour {

    private int frame = 0;
    public float bulletSpeed = 50.0f;
    Transform firePoint;
    Transform gun;
    Transform playerObject;
    private Vector3 playerVec3;
    public Transform bulletPrefab;


	// Use this for initialization
	void Awake () 
    {
		playerObject = GameObject.FindWithTag("Player").transform;
       
        firePoint = transform.Find("gun/FirePoint");
        if( firePoint == null)
        {
            Debug.Log("No firePoint found");
        }

        gun = transform.Find("gun");
        if( gun == null)
        {
            Debug.Log("No gun found");
        }

        if( playerObject == null)
        {
            Debug.Log("No player found");
        }

	}
	
	// Update is called once per frame
	void Update () 
    {
        playerVec3 = playerObject.position;

        Vector2 playerDirection = playerVec3 - gun.position ;

        if (playerDirection.sqrMagnitude > 0.0f)
        {
            gun.rotation = Quaternion.FromToRotation(Vector2.right, playerDirection);
        }

        frame++;
        if (frame > 60)
        {
            var bulletAngles = firePoint.transform.rotation.eulerAngles;
            var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(bulletAngles));
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.forward * bulletSpeed;

            frame = 0;
        }



	}
}
