using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float haltRange;
    public int deathDelay = 100;
    public float pounceMinimum = 0;
    public float pounceChargeRate = 1;
    public GameObject pounceIndicatorPrefab;
    public float moveSpeed = 1;
    public float chargeMax = 100;
    public float landingThreshold;
    public Image screen;
    public float pounceCharge = 0;
    public Animator playerAnimator;
    public bool gameFinished = false;

    public int collected = 0;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spr;
    private Vector3 spawnPosition;
    private Vector3 mousePosition;
    private Vector3 playerPosition;
    private Vector3 rotation;
    private Vector2 jumpForce;
    private float xOffset;
    private float yOffset;
    private string playerState; // stores movement state
    private int deathTimer;
    private GameObject pounceIndicator;
    private int chargeDirection = 1;
    private Scene thisScene;


    


    void Start() {
        spawnPosition = transform.position;
        body = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        
        
        playerState = "moving";
        deathTimer = deathDelay;
        if (pounceIndicatorPrefab)
        {
            pounceIndicator = Instantiate(pounceIndicatorPrefab, transform);
        }
        playerAnimator.ResetTrigger("catWalking");
        playerAnimator.ResetTrigger("catPouncing");
        playerAnimator.ResetTrigger("catSitting");

        thisScene = SceneManager.GetActiveScene();
    }

    void Update() {
        if (gameFinished)
            body.bodyType = RigidbodyType2D.Static;

        if (playerState == "dying" || gameFinished) {
            return;
        }


        // read input, change player state accordingly
        if (Input.GetKey("space") && playerState != "aerial") {
            playerAnimator.ResetTrigger("catWalking");
            playerState = "charging";
           playerAnimator.SetTrigger("catSitting");
        } else if (Input.GetKey("z") && playerState != "charging") {
            playerAnimator.ResetTrigger("catWalking");
            playerState = "sitting";
            playerAnimator.SetTrigger("catSitting");
        } else if (Vector3.Distance(mousePosition, playerPosition) <= haltRange) {
            playerState = "still";            
        } else if (Input.GetKeyUp("space")) {
           GetComponent<moreAudioClips>().PlayClip(2);


            playerAnimator.SetTrigger("catPouncing");
            playerState = "pouncing";
        } else if (playerState != "aerial" && playerState != "charging" && playerState != "pouncing") {
            playerState = "walking";
            playerAnimator.ResetTrigger("catSitting");
            playerAnimator.SetTrigger("catWalking");
            
        }

        // check for death
        if (deathTimer <= 0) {
            StartCoroutine(PlayerReset());
        } else {
            deathTimer -= 1;
        }
    }

    private void FixedUpdate() {
        if (playerState == "dying" || gameFinished) {
            return;
        }
        // get positional variables
        playerPosition = transform.position;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 offset = (mousePosition - playerPosition).normalized;

        // act according to player state
        if (playerState == "walking" || playerState == "charging" || playerState == "sitting") {
            // cat should rotate while walking, charging, and sitting
            float rot_z = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }

        if (pounceIndicator != null)
        {
            if (playerState == "charging" || playerState == "pouncing")
            {
                // Predict where the player will land and display a sprite as such.
                pounceIndicator.transform.rotation = this.transform.rotation;

                Vector3 jumpForce = offset.normalized * pounceCharge;
                Vector3 vel = body.velocity;
                vel = vel + jumpForce + (Physics.gravity * Time.fixedDeltaTime);
                Vector2[] spots = Plot(transform.position, vel, 60);

                pounceIndicator.transform.position = spots[spots.Length - 1];
                pounceIndicator.SetActive(true);
            }
            else
            {
                pounceIndicator.SetActive(false);
            }
        }

        if (playerState == "walking") {
            
            if (Vector2.Distance(mousePosition, playerPosition) < 0.1f) {
                body.velocity = new Vector2(0, 0);
            } else {
                offset.Normalize();
               
               
                
                body.AddForce(offset * moveSpeed * Time.deltaTime);
                
                
            }
            // spr.color = Color.white;
        } else if (playerState == "sitting") {
            
            // spr.color = Color.white;
        } else if (playerState == "charging") {
           
            if (pounceCharge >= chargeMax) {
                chargeDirection = -1;
            } else if(pounceCharge <= 0) {
                chargeDirection = 1;
            }
            pounceCharge += pounceChargeRate * chargeDirection;
            // spr.color = Color.yellow;
        } else if (playerState == "pouncing") {
            
            // Debug.Log("Pounced! Charge: " + pounceCharge.ToString());
            jumpForce = offset.normalized * pounceCharge;
            body.AddForce(jumpForce, ForceMode2D.Impulse);
            pounceCharge = pounceMinimum;
            playerState = "aerial";
            playerAnimator.ResetTrigger("catSitting");
        } else if (playerState == "aerial") {
            // spr.color = Color.blue;
            if (body.velocity.magnitude < landingThreshold) {
                playerState = "walking";
            }
        }

    }

    public IEnumerator PlayerReset() {
        GetComponent<moreAudioClips>().PlayClip(3);
        deathTimer = deathDelay;
        playerState = "dying";
        Color fixedColor = screen.color;
        fixedColor.a = 1;
        screen.color = fixedColor;
        screen.CrossFadeAlpha(0f, 0f, true);
        screen.CrossFadeAlpha(1, 0.25f, false);
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene("Level1");
        
        /*
        transform.position = spawnPosition;
        screen.CrossFadeAlpha(0f, 0.25f, true);
        playerState = "walking";
        */
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (playerState == "aerial" || collision.gameObject.layer >= 9) {
            deathTimer = deathDelay;
            
        }

        if (collision.gameObject.layer == 10) {
            if (collision.gameObject.GetComponent<platformController>() != null) {
                platformController platform = collision.gameObject.GetComponent<platformController>();
                transform.position += platform.velocity * 1.1f;
            } else if(collision.gameObject.GetComponent<carController>() != null) {
                carController platform = collision.gameObject.GetComponent<carController>();
                transform.position += platform.velocity * 1.1f;
            }
        }

        if (collision.gameObject.CompareTag("Collectable"))
        {
            GetComponent<moreAudioClips>().PlayClip(0);
            collision.gameObject.SetActive(false);
        }
    }

    private Vector2[] Plot(Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * body.gravityScale * timestep * timestep;
        float drag = 1f - timestep * body.drag;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; ++i)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }

        return results;
    }

    public void SetFinished() {
        gameFinished = true;
    }
}
