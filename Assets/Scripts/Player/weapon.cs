using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    bool canShoot = true;
    private float lastShot;
    public float rateOfFire = 5.0f;
    public float bulletSpeed = 50.0f;
    public Transform bulletPrefab;
    public Transform muzzleFlashPrefab;

    Transform firePoint;
    public float bulletSpawnSpray = 0.1f;

    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.Log("No firePoint found");
        }
    }

    void Update()
    {
        if (!canShoot)
            return;
        //Auto firing mode
        Shoot();    
        lastShot = Time.time;
        StartCoroutine(HandleROF()); //Handles rate of fire
    }

    void Shoot()
    {

        //muzzleflash
        var muzzleAngles = transform.rotation.eulerAngles;
        Transform clone = Instantiate(muzzleFlashPrefab, firePoint.position, Quaternion.Euler(muzzleAngles)) as Transform;
        clone.parent = firePoint;
        Destroy(clone.gameObject, 0.2f);

        //bullet
        var bulletAngles = transform.rotation.eulerAngles;
        var bullet = Instantiate(bulletPrefab, firePoint.position + new Vector3(0, Random.Range(-bulletSpawnSpray, bulletSpawnSpray), 0), Quaternion.Euler(bulletAngles));
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0); 
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


