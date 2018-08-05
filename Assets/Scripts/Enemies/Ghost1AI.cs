using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost1AI : MonoBehaviour
{
    private Rigidbody2D myRB;
    private ParticleSystem myPS;
    private ParticleSystem myAttack1PS;
    private ParticleSystem myAttack2PS;
    private CapsuleCollider2D myCC;
    private Animator myAnim;
    private SpriteRenderer mySprite;
    // GameObject wavyChild;

    private Vector3 playerTransform;
    private Vector3 playerDirection;
    private float xDif;
    private float yDif;
    private float frame;
    private float frameAnim;

    private GameObject ghost_attack1;
    private GameObject ghost_attack2;




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
    [Space(10)]

    public float trailEmitStopSpeed = 1f;
    public GameObject attackChargePS;

    public float enterBoundary = 3.85f;

    public float randomnessAmount = 1f;

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myCC = GetComponent<CapsuleCollider2D>();
        myPS = GetComponent<ParticleSystem>();
        mySprite = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();

        //wavyChild = gameObject.transform.Find("ghost1Wavy").gameObject;

        ghost_attack1 = gameObject.transform.Find("ghost_attack1").gameObject;
        ghost_attack2 = gameObject.transform.Find("ghost_attack2").gameObject;

        myAttack1PS = gameObject.transform.Find("ghost_attack1").GetComponent<ParticleSystem>();
        myAttack2PS = gameObject.transform.Find("ghost_attack2").GetComponent<ParticleSystem>();


        particlesOff();


        myCC.enabled = false;

        isAttacking = false;
        isIdle = false;
        isEntering = true;

        enter();
    }


    void enter()
    {
        myCC.enabled = false;
        mySprite.enabled = true;

        isAttacking = false;
        isIdle = false;
        isEntering = true;

        Physics2D.IgnoreLayerCollision(14,10,false);

        Invoke("idle", enterTimeWait);
    }

    void idle()
    {
        myCC.enabled = true;
        mySprite.enabled = true;
        myPS.Stop();

        isAttacking = false;
        isIdle = true;
        isEntering = false;

        Physics2D.IgnoreLayerCollision(14,10,false);


        particlesOff();


        StopCoroutine("backToIdleTimer");
        StopCoroutine("attackTimer");

        StartCoroutine("idleMovement");
        StartCoroutine("attackTimer");

    }

    void particlesOn()
    {

//        ghost_attack1.SetActive(true);
//        ghost_attack2.SetActive(true);

//        myAttack1PS.Play();
//        myAttack2PS.Play();
    }
    void particlesOff()
    {
//        ghost_attack1.SetActive(false);
//        ghost_attack2.SetActive(false);
////
//        myAttack1PS.Stop();
//        myAttack2PS.Stop();
//
//        myAttack1PS.Clear();
//        myAttack2PS.Clear();
//
 //       myAttack1PS.emission.enabled

//        myAttack1PS.emission.enabled = false;
//        myAttack2PS.emission.enabled = false;
//        myAttack1PS.emission.enabled(false);
//        myAttack2PS.emission.enabled(false);

    }

    IEnumerator idleMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleMovementTimer + Random.Range(-randomnessAmount,randomnessAmount));
            isAttacking = false;
            isIdle = true;
            isEntering = false;
        }
    }

    IEnumerator attackTimer()
    {
        //stop moving
        //particle effect
        if (isAttacking)
        {
            isIdle = false;
            isEntering = false;
            yield break;
        }
        yield return new WaitForSeconds(attackTimeBetweenAttacks + Random.Range(-randomnessAmount,randomnessAmount));
        
        Attack();
        yield break;
    }

    void Attack()
    {
        StopCoroutine("idleMovement");

        isAttacking = true;
        isIdle = false;
        isEntering = false;

        particlesOn();

        StartCoroutine("backToIdleTimer");  

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

        playerTransform = GameObject.FindWithTag("Player").transform.position;  
        distanceToPlayer = Vector3.Distance(playerTransform, transform.position);

        xDif = playerTransform.x - transform.position.x;
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

            // if they're on the screen then turn the collision on
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
                myRB.AddForce(playerTransform - transform.position * idleSpeed);
            }
            if (distanceToPlayer < idleStopDistance && distanceToPlayer > disperseDistance)
            {
                myRB.velocity = myRB.velocity * idleRelaxSpeed;
            }
            if (distanceToPlayer < disperseDistance + Random.Range(-randomnessAmount,randomnessAmount))
            {
                myRB.AddForce((playerTransform + transform.position) * disperseIdleSpeed);
            }
            if (myRB.velocity.magnitude > idleMaxSpeed)
            {
                myRB.velocity = myRB.velocity * idleMaxSpeed;
                isAttacking = false;
            }
        }

        if (isAttacking)
        {
//                ghost_attack1.SetActive(true);
//                ghost_attack2.SetActive(true);


//            if (myAttack1PS.isPlaying == false)
//            {
//                myAttack1PS.Play();
//            }
//
//            if (myAttack2PS.isPlaying == false)
//            {
//                myAttack2PS.Play();
//            }

                Physics2D.IgnoreLayerCollision(14,10,true);
                mySprite.enabled = false;
                myPS.Play();

                myRB.AddForce((playerTransform.normalized - transform.position.normalized) * attackSpeed);
                if (myRB.velocity.magnitude > idleMaxSpeed)
                {
                    myRB.velocity = myRB.velocity.normalized * attackMaxSpeed;
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
            if (myRB.velocity.x > 0.01f)
            {
                myAnim.SetBool("facingRight",true);


                if (isAttacking == true)
                {
                    mySprite.enabled = false;
                    var psFlip = myPS.textureSheetAnimation;
                    psFlip.flipU = 1;

                }

            }
            else if (myRB.velocity.x < -0.01f)
            {
                myAnim.SetBool("facingRight", false);
                //myRend.material.SetTextureScale("_MainTex", new Vector2(-1,1));
                //myWavy.texture.Resize(-1, 1);
                //myWavy.texture.blackTexture;
                if (isAttacking == true)
                {
                    mySprite.enabled = false;
                    var psFlip = myPS.textureSheetAnimation;
                    psFlip.flipU = 0;

                }
            }
            else// if (myRB.velocity.x == 0)
            {
                if (isIdle == true && mySprite.enabled == false)
                {
                    mySprite.enabled = true;
                }

            }
            frameAnim = 0;
        }
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

