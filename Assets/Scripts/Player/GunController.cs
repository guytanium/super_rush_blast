using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class GunController : MonoBehaviour
{
    public bool isFiring;

    public float fireRate;
    private float shotCounter;

    bool canShoot = true;
    private float lastShot;
    public float rateOfFire = 5.0f;
    public float bulletSpeed = 50.0f;
    public Transform bulletPrefab;
    public Transform muzzleFlashPrefab;

    Transform firePoint;
    public float bulletSpawnSpray = 0.1f;

    private ParticleSystem gunShell;

    void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.Log("No firePoint found");
        }
        gunShell = GetComponent<ParticleSystem>();
    }

    // Use this for initialization
    void Start()
    {
		
    }
	
    // Update is called once per frame
    void Update()
    {
        Vector2 playerDirection = Vector2.right * CrossPlatformInputManager.GetAxisRaw("RHorizontal") + Vector2.up * CrossPlatformInputManager.GetAxisRaw("RVertical");

        if (playerDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.FromToRotation(Vector2.right, playerDirection);
        }

        if (Mathf.Abs(CrossPlatformInputManager.GetAxisRaw("RHorizontal")) > 0.2f || Mathf.Abs(CrossPlatformInputManager.GetAxisRaw("RVertical")) > 0.2f)
        {

            if (canShoot)
            {
                Shoot();
                lastShot = Time.time;
                StartCoroutine(HandleROF());
            }
            return;
        }
    }

    void Shoot()
    {
        gunShell.Emit(1);

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
