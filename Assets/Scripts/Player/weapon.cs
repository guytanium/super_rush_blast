using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour 
//{
//    public float fireRate = 1f; // bullets per second
//
//    public Transform projectilePrefab;
//
//    private float _lastFireTime;
//
//    private void Update() {
////        if (Input.GetKey("F") && CanFireProjectile)
//        if (CanFireProjectile)
//
//            FireProjectile();
//    }
//
//    public bool CanFireProjectile {
//        get { return Time.time - _lastFireTime > 1f / fireRate; }
//    }
//
//    public void FireProjectile() {
//        _lastFireTime = Time.time;
//
//        // etc...
//    }
{
    bool canShoot = true;
    private float lastShot;
    public float rateOfFire = 5.0f;
    public float bulletSpeed = 50.0f;
    public Transform bulletPrefab;
    public Transform muzzleFlashPrefab;

    Transform firePoint;
    public float bulletSpawnSpray = 0.1f;

//    private new Vector3 bulletSpawnRandom;


    void Awake()
    {

//        bulletSpawnRandom = new Vector3(Random.Range(100.0F, 1000.0F), 70, Random.Range(100.0F, 1000.0F));


        firePoint = transform.Find("FirePoint");
        if( firePoint == null)
        {
            Debug.Log("No firePoint found");
        }

    }


    void Update()
    {
        if (!canShoot)
            return;

        //Auto firing mode
//        if (Input.GetMouseButton(0))
//        {
            Shoot();    //Shoot
            lastShot = Time.time;
            StartCoroutine(HandleROF()); //Handles rate of fire
//        }
    }


    void Shoot()
    {

        //muzzleflash
        var muzzleAngles = transform.rotation.eulerAngles;
        Transform clone = Instantiate(muzzleFlashPrefab, firePoint.position,Quaternion.Euler(muzzleAngles)) as Transform;
        clone.parent = firePoint;
//        float size = Random.Range(0.7f, 1.0f);
//        clone.localScale = new Vector3(size, size, 1);
        Destroy (clone.gameObject, 0.2f);

        //bullet
        var bulletAngles = transform.rotation.eulerAngles;
        var bullet = Instantiate(bulletPrefab, firePoint.position+new Vector3(0, Random.Range(-bulletSpawnSpray, bulletSpawnSpray), 0), Quaternion.Euler(bulletAngles));
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed,0); //bullet.transform.forward * bulletSpeed;
    }

    IEnumerator HandleROF()
    {
        //Set can shoot to false
        canShoot = false;

        lastShot += (float)1 / rateOfFire;

        //Wait until enough time has passed
        while (Time.time < lastShot)
            yield return null;

        //When enough time has passed, allow us to shoot again
        canShoot = true;
    }


}


