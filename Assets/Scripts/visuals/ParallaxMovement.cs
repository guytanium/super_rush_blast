﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMovement : MonoBehaviour
{

    public float scrollSpeed;
	
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime * scrollSpeed;

    }
}
