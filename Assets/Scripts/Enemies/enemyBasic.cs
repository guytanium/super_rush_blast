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

    //[SerializeField] //debug that it's assigned to camera
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

//
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class enemyBasic : MonoBehaviour
//{
//    public float health = 50.0f;
//    Material originalMaterial;
//    Texture2D originalTexture;
//    WavySprite ghostlyChildOfMine;
//    public GameObject wavyChild;
//    bool flashing;
//    bool isSprite = true;
//
//    public GameObject gibsParticleSystem;
//    public GameObject explosionParticleSystem;
//
//    //[SerializeField] //debug that it's assigned to camera
//    private GameObject shakeObject;
//    public ShakeTransformEventData shakeEventOnDie;
//
//
//
//    void Start()
//    {
//        shakeObject = GameObject.Find("CameraShake");
//
//        if (GetComponent<SpriteRenderer>() != null)
//        {
//            originalMaterial = GetComponent<SpriteRenderer>().material;
//            isSprite = true;
//        }
//        else //if (GetComponentInChildren<WavySprite>().texture != null)
//        {
//            //ghostlyChildOfMine = gameObject.transform.GetChild(0).gameObject.GetComponent<WavySprite>();
//            //originalTexture = gameObject.transform.GetChild(0).gameObject.GetComponent<WavySprite>().texture;
//
//            ghostlyChildOfMine = wavyChild.GetComponent<WavySprite>();
//            originalTexture = ghostlyChildOfMine.texture;
//            isSprite = false;
//        }
//
//    }
//
//    public void FlashWrapper()
//    {
//        if (!flashing)
//            StartCoroutine("Flash");
//    }
//
//
//
//    IEnumerator Flash()
//    {
//
//        if (isSprite == true)
//        {
//            flashing = true;
//            GetComponent<SpriteRenderer>().material = Resources.Load("SpriteWhite", typeof(Material)) as Material; //flashMaterial;
//            yield
//            return new WaitForSeconds(0.05f);
//            GetComponent<SpriteRenderer>().material = originalMaterial;
//            yield
//            return new WaitForSeconds(0.05f);
//            flashing = false;
//        }
//        else if (isSprite == false)
//        {
//            flashing = true;
//            ghostlyChildOfMine.texture = Resources.Load("ghost1white", typeof(Texture)) as Texture2D; //flashMaterial;
//            yield
//            return new WaitForSeconds(0.05f);
//            GetComponentInChildren<WavySprite>().texture = originalTexture;
//            ghostlyChildOfMine.texture = originalTexture;
//            yield
//            return new WaitForSeconds(0.05f);
//            flashing = false;
//
//        }
//    }
//
//    public void Damage()
//    {
//        health--;
//        FlashWrapper();
//        if (health <= 0)
//        {
//            Die();
//        }
//    }
//
//    void Die()
//    {
//        if (gibsParticleSystem != null)
//        {
//            Debug.Log("gibs should play");
//            GameObject gibs = Instantiate(gibsParticleSystem, transform.position + new Vector3(0, 0, 2), transform.rotation) as GameObject;
//            ParticleSystem gibsParts = gibs.GetComponent<ParticleSystem>();
//            Destroy(gibs, gibsParts.main.duration);
//        }
//
//        if (explosionParticleSystem != null)
//        {
//            GameObject explosion = Instantiate(explosionParticleSystem, transform.position + new Vector3(0, 0, 2), transform.rotation) as GameObject;
//            ParticleSystem explosionParts = explosion.GetComponent<ParticleSystem>();
//            Destroy(explosion, explosionParts.main.duration);
//        }
//
//
//        shakeObject.GetComponent<ShakeTransform>().AddShakeEvent(shakeEventOnDie);
//
//        Destroy(gameObject);
//    }
//
//
//    //void OnTriggerEnter2D(Collider2D col)
//    void OnCollisionEnter2D(Collision2D col)
//    {
//
//        if (col.gameObject.tag == "Player")
//        {
//            col.gameObject.GetComponent<PlayerController>().Damage();
//
//
//        }
//    }
//
//}
//
//
