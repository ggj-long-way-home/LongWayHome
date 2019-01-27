using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class carController : raycastController
{

    public LayerMask passengerMask;
    public platformTypeInfo platformType;

    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;

    public bool facingRight;
    public float speed;
    public bool cyclic;
    public bool isSticky;
    public float waitTime;
    [Range(0, 2)]
    public float easeAmount;
    public Vector3 velocity;

    int fromWaypointIndex;
    float percentBetweenWaypoints;
    float nextMoveTime;


    List<PassengerMovement> passengerMovement;
    

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
        if (speed == 0)
        {
            platformType.isMoving = false;

        }
        else
        {
            platformType.isMoving = true;
        }
    }

    // Update is called once per frame
    void Update()
    {



        velocity = CalculatePlatformMovement();


        transform.Translate(velocity);

    }

    public float getVelocityX()
    {
        return velocity.x;
    }

    float Ease(float x)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    Vector3 CalculatePlatformMovement()
    {
        if (transform.position.x < -9)
        {
            return new Vector3(21, 0, 0);
        }
        if (transform.position.x > 12)
        {
            return new Vector3(-21, 0, 0);
        }
        if (transform.position.x < 12 && facingRight)
        {
            return new Vector3(speed * Time.deltaTime, 0, 0);
        }
        else
        {
            return new Vector3(-speed * Time.deltaTime, 0, 0);
        }
    }

   

    void CalculatePassengerMovement(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);



        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                if (hit)
                {
                    if (hit.distance == 0)
                    {
                        continue;
                    }
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;

                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }

            }
        }
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                Vector2 rayOriginOther = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                rayOriginOther += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);
                RaycastHit2D hitOther = Physics2D.Raycast(rayOriginOther, Vector2.right * -directionX, rayLength, passengerMask);
                Debug.DrawRay(rayOriginOther, Vector2.right * -directionX * rayLength, Color.red);
                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = velocity.x - (hit.distance - skinWidth);
                        float pushY = -skinWidth;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));

                    }
                }

                if (hitOther)
                {

                    if (!movedPassengers.Contains(hitOther.transform))
                    {
                        movedPassengers.Add(hitOther.transform);
                        float pushX;

                        if (isSticky)
                        {
                            pushX = velocity.x - (hitOther.distance - skinWidth);
                        }
                        else
                        {
                            pushX = 0;
                        }
                        float pushY = -skinWidth;


                        passengerMovement.Add(new PassengerMovement(hitOther.transform, new Vector3(pushX, pushY), false, false));
                    }
                }
            }
        }


        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));


                    }
                }
            }
        }
    }

    struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }



    public struct platformTypeInfo
    {
        public bool isBouncy;
        public bool isFading;
        public bool isMoving;
        public bool movingRight;
    }



}



