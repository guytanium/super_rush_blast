using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]



public class Tiling : MonoBehaviour
{
    //for specific objects just to parallax by without repeating
    public bool isTiling = true;

    public int offsetX = 2;
    // just offset the edge of the screen to prevent pop-in or visual blips
    public bool hasARightBuddy = false;
    // do we need to instantiate stuff?
    public bool hasALeftBuddy = false;
    // as above
    public bool reverseScale = false;
    //used if the object is not tileable
    private float spriteWidth = 0.0f;
    //with of our element
    private Camera cam;
    private Transform myTransform;

    public float scrollSpeed;

    void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }

    // Use this for initialization
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;

    }
	
    // Update is called once per frame
    void Update()
    { 
        transform.position += transform.right * Time.deltaTime * -scrollSpeed;
        if (isTiling == true)
        {
            // does it still need buddies, if not do nothing
            if (hasALeftBuddy == false || hasARightBuddy == false)
            {
                // calculate the cameras extent meaning half the width of what the camera can see
                // and it's going to be in world coordinates rather than pixels
                float camHorizontalExtent = cam.orthographicSize * Screen.width / Screen.height;

                //calculate the x position where the camera can see the edge of the sprite
                float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtent;
                float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtent;

                // checking if the position of the camera is bigger or equal to the edge distance
                // then minusing the offset and checking if there's already an instance
                if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
                {
                    makeNewBuddy(1);
                    hasARightBuddy = true;
                }
                else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false)
                {
                    makeNewBuddy(-1);
                    hasALeftBuddy = true;
                }
            }
        }      
    }


    //creates a buddy on the correct edge as required
    void makeNewBuddy(int rightOrLeft)
    {
        //calculating the psotion for the new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        // instantiating our new buddy and storing them in a variable
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;


        //this is for mirroring! correctly tiling images dont to scale the x axis
        if (reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);

        }

        // keep the heirarchy structured well
        newBuddy.parent = myTransform.parent;

        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }

    }

    void OnBecameInvisible()
    { 
        if (gameObject.transform.position.x < 0)
        {
            Destroy(gameObject);
        }
    }

}
