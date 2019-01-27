using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    float st, angle;
    Rigidbody2D body;
    public bool moveY;
    Vector3 followPos, dir, vAng; //yourself; relationship bt you and the cat
    public GameObject cat;
    public float followSpeedXMax, followSpeedYMax, followStamina,
        followDeltaA, accelX, accelY, speedX, speedY, force;

    // Start is called before the first frame update
    void Start( )
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        followPos = transform.position;
        st = followStamina; //keep track of stamina
        speedX = 0;
        speedY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 acc = new Vector3(accelX, accelY, 0.0f);

        //angle bt you and the cat
        dir = transform.position - cat.transform.position;
        dir.Normalize();
        body.AddForce(dir * force);
        //angle = Mathf.Atan2(vAng.y, vAng.x);

        //adjust acceleration
        //accelX *= Mathf.Cos(angle);
        //print(Mathf.Cos(angle));
        //accelY *= Mathf.Sin(angle);

        //move
        followPos.x += speedX;
        followPos.y += speedY;
        transform.position = followPos;

        //cap speed
        if (speedX <= followSpeedXMax && speedX > (-1)*followSpeedXMax)
            accelX += followDeltaA;
        if (speedY <= followSpeedYMax && moveY == true && speedY > (-1) * followSpeedYMax)
            accelY += followDeltaA;

        if (st <= 0) //if you run out of stamina
        {
            
            accelX -= followDeltaA;
            if (moveY ==true) accelY -= followDeltaA;
        }

        st--; //stamina counter


        //accelerate
        speedX += accelX;
        speedY += accelY;
        
    }
}
