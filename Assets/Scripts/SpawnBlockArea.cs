using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlockArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        //all projectile colliding game objects should be tagged "Enemy" or whatever in inspector but that tag must be reflected in the below if conditional
        if (col.gameObject.tag == "Spawn")
        {
            col.gameObject.GetComponent<SpawnPoint>().SpawnDisable();
        }
    }
}
