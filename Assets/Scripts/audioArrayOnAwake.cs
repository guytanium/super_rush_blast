﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioArrayOnAwake : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<AudioRandomArray>().PlaySound();
    }
}
