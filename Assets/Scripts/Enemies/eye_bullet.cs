using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eye_bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 1.0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * speed);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //all projectile colliding game objects should be tagged "Enemy" or whatever in inspector but that tag must be reflected in the below if conditional
        if (col.gameObject.tag == "Player")
        {
            //add an explosion or something
            //destroy the projectile that just caused the trigger collision
            Destroy(gameObject);


        }
    }

}
