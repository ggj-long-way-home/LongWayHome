using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    //note: freeze rotation in the z direction

    public float trackForce, t, dF;
    public GameObject cat;
    public bool dead;

    private Rigidbody2D body;
    private float endTime;

    // Start is called before the first frame update
    void Start()
    {
        endTime = 0.0f;
        body = gameObject.GetComponent<Rigidbody2D>();
        //prevent rotation
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        t = GetComponent<timer>().timeLeft / 4;
        dF = trackForce / t;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = cat.transform.position - transform.position;
        dir.Normalize();
        body.AddForce(dir * trackForce);
        float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);

        //if you couldn't catch the cat, the enemy stops moving 
        if (GetComponent<timer>().outOfTime())
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        //if you run into the cat
        if (GetComponent<Collider2D>().IsTouching(cat.GetComponent<Collider2D>()))
        {
            print("the enemy killed the cat");
            GetComponent<timer>().resetTimer();
            
            StartCoroutine(cat.GetComponent<PlayerController>().PlayerReset());
            
        }


        // 3/4 through the time, the force constantly decreases s.t. its a=0 at t=0
        if (GetComponent<timer>().timeLeft <= t && GetComponent<timer>().timeLeft >= 0)
        {
            if ((Time.time - endTime) > 1)
            {
                endTime = Time.time;
                trackForce -= dF;
            }
        }
    }

    public bool isDead()
    {
        return dead;
    }
}
