using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

    private Rigidbody2D myRB;
    private Animator myAnim;

    [Header("Movement")]
    public float moveSpeed;
    public float moveDeadZone = 0.19f;
    private float horizontalInputClean;
    private float verticalInputClean;
    private Vector2 moveInput;
    private Vector2 moveVelocity;

    [Header("Health")]
    public float playerStartingHealth;
    public static float playerHealth;
    public Slider healthSlider;
    public float invincibilityTime;
    public float invincibilityFlashRate;
    [SerializeField]
    private bool isInvincible;

    [Header("Etc")]
    private GameObject shakeObject;
    public ShakeTransformEventData data;
    Material originalMaterial;
    private bool flashing;
    private SpriteRenderer mySR;
    public Image blackImg;
    public Animator blackImgAnim;


    // Use this for initialization
    void Awake()
    {
        shakeObject = GameObject.Find("CameraShake");

        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        mySR = GetComponent<SpriteRenderer>();

        playerHealth = playerStartingHealth;

        isInvincible = false;

        originalMaterial = GetComponent<SpriteRenderer>().material;

        blackImg = GameObject.Find("BlackImg").GetComponent<Image>();
        blackImgAnim = GameObject.Find("BlackImg").GetComponent<Animator>();
    }

    void Start()
    {
        blackImgAnim.SetBool("Fade", false);

    }
	
    // Update is called once per frame
    void Update()
    {


        //deadzone
        if (Mathf.Abs(CrossPlatformInputManager.GetAxisRaw("Horizontal")) > moveDeadZone)
        {
            horizontalInputClean = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            if (horizontalInputClean > 0)
            {
                myAnim.SetBool("facingRight", true);
            }
            if (horizontalInputClean < 0)
            {
                myAnim.SetBool("facingRight", false);
            }
        }
        else
        {
            horizontalInputClean = 0.0f;
        }

        if (Mathf.Abs(CrossPlatformInputManager.GetAxisRaw("Vertical")) > moveDeadZone)
        {
            verticalInputClean = CrossPlatformInputManager.GetAxisRaw("Vertical");
        }
        else
        {
            verticalInputClean = 0.0f;
        }

        moveInput = new Vector2(horizontalInputClean, verticalInputClean);
        moveVelocity = moveInput * moveSpeed;

        if (isInvincible == true)
        {
            StartCoroutine("InvincibilityFlashing");
        }
    }

    void FixedUpdate()
    {
        myRB.velocity = moveVelocity;
    }

    public void Damage()
    {
        if (isInvincible == false)
        {

            playerHealth--;
            healthSlider.value = playerHealth;
            Debug.Log("player is at " + playerHealth + " health");
            if (playerHealth <= 0)
            {
                Die();
            }

            shakeObject.GetComponent<ShakeTransform>().AddShakeEvent(data);

            StartCoroutine("InvincibilityTimer");
        }

    }

    void Die()
    {
        //player ded
        //TODO: make player die, restart level, whatever
        mySR.enabled = false;
        StartCoroutine("GameOverTimer");

    }



    IEnumerator GameOverTimer()
    {
        yield return new WaitForSeconds(1);
        blackImgAnim.SetBool("Fade", true);
        SceneManager.LoadScene("GameOver");
        yield break;
    }

    IEnumerator InvincibilityTimer()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);

        isInvincible = false;

        StopCoroutine("InvincibilityFlashing");
        GetComponent<SpriteRenderer>().material = originalMaterial;
        yield break;

    }

    IEnumerator InvincibilityFlashing()
    {
        if (isInvincible == true)
        {
            GetComponent<SpriteRenderer>().material = Resources.Load("SpriteWhite", typeof(Material)) as Material; //flashMaterial;
            yield
            return new WaitForSeconds(invincibilityFlashRate);
            GetComponent<SpriteRenderer>().material = originalMaterial;
            yield
            return new WaitForSeconds(invincibilityFlashRate);
        }
    }
}
