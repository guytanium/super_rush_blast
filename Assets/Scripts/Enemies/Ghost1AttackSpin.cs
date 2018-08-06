using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost1AttackSpin : MonoBehaviour
{

    private Transform parent;
    public float orbitSpeed;
    public bool isSecondOrbit;
    private float secondOrbitOffset;

    // Use this for initialization
    void Start()
    {
        if (isSecondOrbit == true)
        {
            secondOrbitOffset = orbitSpeed * 2;
        }
    }
	
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(Mathf.Cos((Time.time * orbitSpeed) - secondOrbitOffset), Mathf.Sin((Time.time * orbitSpeed) - secondOrbitOffset), 0);
    }
	
}
