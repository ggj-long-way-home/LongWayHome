
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotate : raycastController
{
    public float moveDelay;
    public CollisionInfo collisions;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private Vector3 mousePosition;
    private Vector3 playerPosition;
    private float xOffset;
    private float yOffset;
    private float oldDirectionx;
    private float oldDirectiony;

    // Start is called before the first frame update
    public override void Start()
    {
        
        collisions.Reset();
        collisions.faceDir = 1;
        body = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // get mouse position, create vector that is difference of mouse position and player position

       


        playerPosition = transform.position;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if ((Mathf.Sign((mousePosition.x - playerPosition.x)) != oldDirectionx))
        {

            collisions.left = false;
            collisions.right = false;
            collisions.hitWallhorz = false;
        }

        if (Mathf.Sign((mousePosition.y - playerPosition.y)) != oldDirectiony)
        {
            collisions.above = false;
            collisions.below = false;
            collisions.hitWallvert = false;
        }

        if (collisions.hitWallhorz == true)
        {

        }

        if ((collisions.hitWallhorz == false && collisions.left == false))
        {
            xOffset = mousePosition.x - playerPosition.x;

        }
        else
        {
            Debug.Log(collisions.left);
            xOffset = 0;
        }
        if ((collisions.hitWallvert == false && collisions.below == false) || collisions.hitWallhorz == false)
        {
            yOffset = mousePosition.y - playerPosition.y;
        }
        else
        {
            yOffset = 0;
        }


        playerPosition.x += xOffset * (1.0f / moveDelay); // speed of movement is proportional to how far mouse is
        playerPosition.y += yOffset * (1.0f / moveDelay);

        HorizontalCollisions(xOffset * (1.0f / moveDelay));
        VerticalCollisions(yOffset * (1.0f / moveDelay));

        transform.position = playerPosition;
        Vector3 offset = mousePosition - playerPosition;
        if (collisions.hitWallhorz == false && collisions.hitWallvert == false)
        {
            transform.rotation = Quaternion.LookRotation(offset, Vector3.forward);
        }
    }

    void HorizontalCollisions(float moveAmountx)
    {
        float directionX = Mathf.Sign(mousePosition.x - transform.position.x);

        float rayLength = Mathf.Abs(moveAmountx);

        if (Mathf.Abs(moveAmountx) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);


            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                collisions.hitWallhorz = true;



                if (Mathf.Sign(hit.transform.position.x - transform.position.x) == -1)
                    collisions.left = true;


                if (hit.distance > 2.5 * skinWidth)
                {
                    playerPosition.x = transform.position.x + hit.distance * Mathf.Sign(moveAmountx);
                }
                else
                {
                    playerPosition.x = transform.position.x;
                }

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
        oldDirectionx = directionX;
    }

    void VerticalCollisions(float moveAmounty)
    {

        float directionY = Mathf.Sign(mousePosition.y - transform.position.y);
        float rayLength = Mathf.Abs(moveAmounty);


        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmounty);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {

                collisions.hitWallvert = true;
                if (directionY == 1)
                {
                    collisions.above = true;
                }
                else
                {
                    collisions.below = true;
                }
                if (hit.distance > 2.5 * skinWidth)
                {
                    playerPosition.y = transform.position.y + hit.distance * Mathf.Sign(moveAmounty);
                }
                else
                {
                    playerPosition.y = transform.position.y;
                }
            }

        }
        oldDirectiony = directionY;
    }


    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public float faceDir;
        public bool hitWallhorz;
        public bool hitWallvert;

        public platformController collisionObject;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            hitWallhorz = false;
            hitWallvert = false;

        }
    }



}
