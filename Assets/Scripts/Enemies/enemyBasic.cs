using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBasic : MonoBehaviour
{
    public float health = 50.0f;
    Material originalMaterial;
    Texture2D originalTexture;
    bool flashing;

    public GameObject gibsParticleSystem;
    public GameObject explosionParticleSystem;

    private GameObject shakeObject;
    public ShakeTransformEventData shakeEventOnDie;



    void Start()
    {
        shakeObject = GameObject.Find("CameraShake");
        originalMaterial = GetComponent<SpriteRenderer>().material;
    }

    public void FlashWrapper()
    {
        if (!flashing)
            StartCoroutine("Flash");
    }

    IEnumerator Flash()
    {

        flashing = true;
        GetComponent<SpriteRenderer>().material = Resources.Load("SpriteWhite", typeof(Material)) as Material; //flashMaterial;
        yield
            return new WaitForSeconds(0.05f);
        GetComponent<SpriteRenderer>().material = originalMaterial;
        yield
            return new WaitForSeconds(0.05f);
        flashing = false;
    }

    public void Damage()
    {
        health--;
        FlashWrapper();
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (gibsParticleSystem != null)
        {
            GameObject gibs = Instantiate(gibsParticleSystem, transform.position + new Vector3(0, 0, 2), transform.rotation) as GameObject;
            ParticleSystem gibsParts = gibs.GetComponent<ParticleSystem>();
            Destroy(gibs, gibsParts.main.duration);
        }

        if (explosionParticleSystem != null)
        {
            GameObject explosion = Instantiate(explosionParticleSystem, transform.position + new Vector3(0, 0, 2), transform.rotation) as GameObject;
            ParticleSystem explosionParts = explosion.GetComponent<ParticleSystem>();
            Destroy(explosion, explosionParts.main.duration);
        }
        GameScore.AddPoint(1);
        shakeObject.GetComponent<ShakeTransform>().AddShakeEvent(shakeEventOnDie);
        Destroy(gameObject);


    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().Damage();
        }
    }
}