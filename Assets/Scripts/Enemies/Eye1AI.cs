using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Eye1AI : MonoBehaviour
{
    private Rigidbody2D myRB;
    private CircleCollider2D myCC;
    private Animator myAnim;

    private Transform playerObject;
    private Vector3 playerVec3;
    private Vector3 playerDirection;
    private float xDif;
    private float yDif;
    private float frame;
    private float frameAnim;


    private float distanceToPlayer;

    [Header("Status")]
    [SerializeField]
    private bool isEntering;
    [SerializeField]
    private bool isIdle;
    [SerializeField]
    private bool isAttacking;
    [SerializeField]
    private bool readyToFire;
    [Header("Enter")]

    public float enterTimeWait = 2f;
    [Header("Idle")]

    public float backToIdleWait = 1.5f;
    public float idleStopDistance = 4f;
    public float idleMovementTimer = 1f;
    public float idleSpeed = 1f;
    public float idleRelaxSpeed = 0.25f;
    public float idleMaxSpeed = 1f;
    public float disperseDistance = 1f;
    public float disperseIdleSpeed = 1f;

    [Header("Attack")]
    public float attackChargeUpTime = 1f;
    public float attackTimeBetweenAttacks = 8f;
    public float bulletSpeed = 2.0f;
    public float bulletROF = 0.3f;

    [Space(10)]

    public float trailEmitStopSpeed = 1f;
    public GameObject attackChargePS;

    public float enterBoundary = 3.85f;

    public float randomnessAmount = 1f;



    private Transform retinaPoint;
    private Transform gun;
    public Transform bulletPrefab;
    private SpriteRenderer myRetina;


    void Start()
    {

        retinaPoint = transform.Find("eyeball_pivot/retina");
        if (retinaPoint == null)
        {
            Debug.Log("No retinaPoint found");
        }

        myRetina = retinaPoint.GetComponent<SpriteRenderer>();

        gun = transform.Find("eyeball_pivot");
        if (gun == null)
        {
            Debug.Log("No gun found");
        }

        playerObject = GameObject.FindWithTag("Player").transform;
        if (playerObject == null)
        {
            Debug.Log("No player found");
        }


        myRB = GetComponent<Rigidbody2D>();
        myCC = GetComponent<CircleCollider2D>();
        myAnim = GetComponent<Animator>();
        //myRetina = GetComponentsInChildren<SpriteRenderer>();

        playerObject = GameObject.FindWithTag("Player").transform;
        playerVec3 = playerObject.position;
        //playerVec3 = GameObject.FindWithTag("Player").transform.position;  


        myCC.enabled = false;
        myRetina.enabled = false;


        myAnim.SetBool("attack", false);
        myAnim.SetBool("idle", true);
        myAnim.SetBool("charge", false);
        isAttacking = false;
        isIdle = false;
        isEntering = true;

        enter();
    }


    void enter()
    {
        myCC.enabled = false;
        myRetina.enabled = false;

        isAttacking = false;
        isIdle = false;
        isEntering = true;
        myAnim.SetBool("attack", false);
        myAnim.SetBool("idle", true);
        myAnim.SetBool("charge", false);

        Invoke("idle", enterTimeWait);
    }

    void idle()
    {
        myCC.enabled = true;
        myRetina.enabled = false;

        isAttacking = false;
        isIdle = true;
        isEntering = false;
        myAnim.SetBool("attack", false);
        myAnim.SetBool("idle", true);
        myAnim.SetBool("charge", false);
        readyToFire = false;

        StopCoroutine("backToIdleTimer");
        StopCoroutine("attackChargeUp");
        StopCoroutine("attackTimer");
        StopCoroutine("ROFTimer");

        StartCoroutine("idleMovement");
        StartCoroutine("attackTimer");

    }

    IEnumerator idleMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleMovementTimer + Random.Range(-randomnessAmount, randomnessAmount));
        }
    }

    IEnumerator attackTimer()
    {
        //stop moving
        //particle effect
        if (isAttacking)
        {
            yield break;
        }
        yield return new WaitForSeconds(attackTimeBetweenAttacks + Random.Range(-randomnessAmount, randomnessAmount));

        Attack();
        yield break;
    }

    void Attack()
    {
        isAttacking = true;
        isIdle = false;
        isEntering = false;


        StopCoroutine("attackChargeUp");
        StopCoroutine("idleMovement");

        StartCoroutine("backToIdleTimer");  
        StartCoroutine("attackChargeUp");


    }

    IEnumerator backToIdleTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(backToIdleWait);

            idle();
            yield break;
            //StopCoroutine("backToIdleTimer");
        }
    }

    void FixedUpdate()
    {
        Debug.DrawLine(gameObject.transform.position, myRB.velocity, Color.red);
        //establish all that good stuff we need for the behaviours

        playerVec3 = playerObject.position;

        distanceToPlayer = Vector3.Distance(playerVec3, transform.position);

        xDif = playerVec3.x - transform.position.x;
        yDif = playerDirection.y - transform.position.y;

        playerDirection = new Vector3(xDif, yDif);

        if (isEntering)
        {
            //check that the enemies arent floating off randomly
            if (transform.position.x > 5.5f)
            {
                transform.SetPositionAndRotation(new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), transform.rotation);
            }
            if (transform.position.x < -5.5f)
            {
                transform.SetPositionAndRotation(new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), transform.rotation);
            }
            if (transform.position.y > 5.5f)
            {
                transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), transform.rotation);
            }
            if (transform.position.y < -5.5f)
            {
                transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation);
            }
            if (Vector3.Distance(gameObject.transform.position, Vector3.zero) <= 4.1f)
            {
                myCC.enabled = true;
            }

            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, 1 * Time.deltaTime);
        }
        if (isIdle)
        {
            if (distanceToPlayer > idleStopDistance)
            {
                myRB.AddForce(playerVec3 - transform.position * idleSpeed);
            }
            if (distanceToPlayer < idleStopDistance && distanceToPlayer > disperseDistance)
            {
                myRB.velocity = myRB.velocity * idleRelaxSpeed;
            }
            if (distanceToPlayer < disperseDistance)
            {
                myRB.AddForce((playerVec3 + transform.position) * -disperseIdleSpeed);
            }
            if (myRB.velocity.magnitude > idleMaxSpeed)
            {
                myRB.velocity = myRB.velocity * idleMaxSpeed;
            }
        }

        if (isAttacking)
        {
            myRB.velocity = myRB.velocity * 0;

            playerVec3 = playerObject.position;

            Vector2 playerDirection = playerVec3 - gun.position;

            if (playerDirection.sqrMagnitude > 0.0f)
            {
                gun.rotation = Quaternion.FromToRotation(Vector2.right, playerDirection);
            }

            if (playerVec3.x > transform.position.x)
            {
                myAnim.SetBool("facingRight", true);
            }
            else
            {
                myAnim.SetBool("facingRight", false);
            }
        }
        if (!isIdle && !isAttacking && !isEntering)
        {
            myRB.velocity = myRB.velocity.normalized * 0;
        }

        if (readyToFire == true)
        {
            myRetina.enabled = true;
        }
    }

    void LateUpdate()
    {
        frameAnim++;
        if (frameAnim > 10)
        {
            if (playerObject == null)
            {
                playerObject = GameObject.FindWithTag("Player").transform;  
            }

            if (myRB.velocity.x > 0.01f)
            {
                myAnim.SetBool("facingRight", true);
            }
            else if (myRB.velocity.x < -0.01f)
            {
                myAnim.SetBool("facingRight", false);
            }
            frameAnim = 0;
        }
    }

    IEnumerator ROFTimer()
    {
        while (true)
        {
            var bulletAngles = retinaPoint.transform.rotation.eulerAngles;
            var bullet = Instantiate(bulletPrefab, retinaPoint.position, Quaternion.Euler(bulletAngles));
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.forward * bulletSpeed;
            yield return new WaitForSeconds(bulletROF);
            if (isAttacking == false)
            {
                yield break;
            }
            
        }
    }

    IEnumerator attackChargeUp()
    {
        if (readyToFire == true)
        {
            myRetina.enabled = true;


            myAnim.SetBool("attack", true);
            myAnim.SetBool("idle", false);
            myAnim.SetBool("charge", false);
            yield break;
        }
        myRetina.enabled = false;
        readyToFire = false;

        myAnim.SetBool("attack", false);
        myAnim.SetBool("idle", false);
        myAnim.SetBool("charge", true);

        GameObject chargePS = Instantiate(attackChargePS, transform.position + new Vector3(0, 0, 2), transform.rotation) as GameObject;
        ParticleSystem parts = chargePS.GetComponent<ParticleSystem>();
        Destroy(chargePS, parts.main.duration);

        yield return new WaitForSeconds(attackChargeUpTime);

        readyToFire = true;

        StartCoroutine("ROFTimer");

        myAnim.SetBool("attack", true);
        myAnim.SetBool("idle", false);
        myAnim.SetBool("charge", false);


        yield break;

    }

    void OnColliderEnter()
    {

        if (isEntering)
        {
            Physics2D.IgnoreLayerCollision(13, 11, true);
        }
        if (!isEntering)
        {
            Physics2D.IgnoreLayerCollision(13, 11, false);
        }

    }


}


