using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 1.0f;

    public GameObject bulletImpactSparksPS;
    public GameObject bulletImpactPS;
    public GameObject bulletEnemyImpactPS;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * speed);
    }

    void Update()
    {


    }
    //    void Update()
    //    {
    //        rb.AddForce(transform.right * speed);
    //    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //all projectile colliding game objects should be tagged "Enemy" or whatever in inspector but that tag must be reflected in the below if conditional
        if (col.gameObject.tag == "Enemy")
        {

            GameObject enemyImpactPS = Instantiate(bulletEnemyImpactPS, transform.position + new Vector3(0, 0, 2), transform.rotation);

            col.gameObject.GetComponent<enemyBasic>().Damage();
            ParticleSystem enemyImpactParts = enemyImpactPS.GetComponent<ParticleSystem>();
            Destroy(enemyImpactPS, enemyImpactParts.main.duration);

            //        Destroy(col.gameObject);

        }

        else if (col.gameObject.tag != "Enemy")
        {
            GameObject impactSparksPS = Instantiate(bulletImpactSparksPS, transform.position + new Vector3(0, 0, 2), transform.rotation) as GameObject;
            ParticleSystem sparkParts = impactSparksPS.GetComponent<ParticleSystem>();
            Destroy(impactSparksPS, sparkParts.main.duration);

        }
        GameObject impactPS = Instantiate(bulletImpactPS, transform.position + new Vector3(0, 0, -2), transform.rotation) as GameObject;
                ParticleSystem parts = impactPS.GetComponent<ParticleSystem>();
        Destroy(impactPS, parts.main.duration);

        //destroy the projectile that just caused the trigger collision
        Destroy(gameObject);
    }

}
