using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skull2AI : MonoBehaviour
{
    private Rigidbody2D myRB;
    private CapsuleCollider2D myCC;
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
    public float attackSpeed = 8f;
    public float attackMaxSpeed = 10f;
    public float attackChargeUpTime = 1f;
    public float attackTimeBetweenAttacks = 8f;
    public GameObject myLovelyBones;
    public float boneSpawnRate;
    private bool bonesSpawned;
    [Space(10)]

    public float trailEmitStopSpeed = 1f;
    public GameObject attackChargePS;

    public float enterBoundary = 3.85f;

    public float randomnessAmount = 1f;

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myCC = GetComponent<CapsuleCollider2D>();
        myAnim = GetComponent<Animator>();

        playerObject = GameObject.FindWithTag("Player").transform;
        playerVec3 = playerObject.position;

        myCC.enabled = false;

        myAnim.SetBool("attack", false);
        myAnim.SetBool("idle", true);
        myAnim.SetBool("charge", false);
        isAttacking = false;
        isIdle = false;
        isEntering = true;
        bonesSpawned = false;

        enter();
    }

    void enter()
    {
        myCC.enabled = false;
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

        isAttacking = false;
        isIdle = true;
        isEntering = false;
        myAnim.SetBool("attack", false);
        myAnim.SetBool("idle", true);
        myAnim.SetBool("charge", false);
        bonesSpawned = false;


        StopCoroutine("backToIdleTimer");
        StopCoroutine("attackTimer");

        StartCoroutine("idleMovement");
        StartCoroutine("attackTimer");
    }

    IEnumerator idleMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleMovementTimer + Random.Range(-randomnessAmount,randomnessAmount));
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

        yield return new WaitForSeconds(
            attackTimeBetweenAttacks + 
            Random.Range(-randomnessAmount,randomnessAmount) -
            (Random.Range(1,0)*3)
        );

        Attack();
        yield break;
    }

    void Attack()
    {
        isAttacking = true;
        isIdle = false;
        isEntering = false;


        StopCoroutine("idleMovement");

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
            if (distanceToPlayer < disperseDistance + Random.Range(-randomnessAmount,randomnessAmount))
            {
                myRB.AddForce((playerVec3 + transform.position) * disperseIdleSpeed);
            }
            if (myRB.velocity.magnitude > idleMaxSpeed)
            {
                myRB.velocity = myRB.velocity * idleMaxSpeed;
            }
        }

        if (isAttacking)
        {
            
            myRB.velocity = myRB.velocity * 0;
            if (bonesSpawned == false)
            {
                StartCoroutine("spawnBones");
            }
            if (transform.childCount <= 1)
            {
                idle();

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

        IEnumerator spawnBones()
        {
            bonesSpawned = true;
            int i = 0;
                
            while (i < 4)
            {
                yield return new WaitForSeconds(boneSpawnRate);
                GameObject bonez = Instantiate(myLovelyBones, Vector3.zero, Quaternion.identity) as GameObject;
                bonez.transform.SetParent(gameObject.transform);

                bonez.SendMessage("orbitCounter", i);
                i++;
            }
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
    //    void OnBecameInvisible()
    //    {
    //        //failsafe to make sure no enemies are stuck outside
    //        //TODO: why is this so broken?
    //        if (!isEntering)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }

}



