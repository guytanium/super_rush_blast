using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull2BoneOrbit : MonoBehaviour 
{

    public float rotationSpeed;
    public float orbitSpeed;



    public int orbitCount; 
    private float orbitOffset;
    private float rotationOffset;
    public float releaseTimer;
    private int frame;

    private Vector3 mySkullDaddy;
    private Vector3 myBonePosition;
    public GameObject   boneExplosion;


    private bool isReleased;
    private float newDirection;

	void Start () 
    {
        isReleased = false;
        orbitOffset = orbitCount * 1.5f;
        StartCoroutine(releaseCountdown());
	}

	
	void Update () 
    {

        if (isReleased == false)
        {
//            transform.Rotate(Vector3.forward * Time.deltaTime* (rotationSpeed + rotationOffset)*-1);

            transform.localPosition = new Vector3(Mathf.Cos((Time.time * orbitSpeed) - orbitOffset), Mathf.Sin((Time.time * orbitSpeed) - orbitOffset), 0);
        }
        else if (isReleased == true)
        {

            transform.Translate(((myBonePosition - mySkullDaddy) * Time.deltaTime) * 3, Space.Self);
        }
	}

    IEnumerator releaseCountdown()
    {
        yield return new WaitForSeconds(releaseTimer);

        mySkullDaddy = transform.parent.position;
        myBonePosition = transform.position;
        isReleased = true;

        rotationSpeed = 0;
        orbitSpeed = 0;
        transform.parent = null;
        yield break;
    }

    void orbitCounter (int counter)
    {
        orbitCount = counter;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().Damage();
            Destroy(gameObject);
            Instantiate(boneExplosion);

        }
    }


}
