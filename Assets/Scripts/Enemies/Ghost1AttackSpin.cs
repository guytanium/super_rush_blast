using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost1AttackSpin : MonoBehaviour 
{

    private Transform parent;
    public float orbitSpeed;
    public bool isSecondOrbit;
    private float secondOrbitOffset;

//    private ParticleSystem myPS;


	// Use this for initialization
	void Start () {
        if (isSecondOrbit == true)
        {
            secondOrbitOffset = orbitSpeed * 2;
        }
//        myPS = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () 
    {


       // transform.RotateAround(transform.parent.position, Vector3.forward,1.5f * Time.time);
    


            // planet to spin on it's own axis
          //  transform.Rotate (transform.forward * 5f * Time.deltaTime);

            // planet to travel along a path that rotates around the sun
         //   transform.RotateAround (parent.transform.position, Vector3.right, 1f * Time.deltaTime);



        transform.localPosition = new Vector3(Mathf.Cos ((Time.time * orbitSpeed) -secondOrbitOffset ),Mathf.Sin ((Time.time * orbitSpeed) - secondOrbitOffset),0);

//        myPS.Play();

        }
	
}
