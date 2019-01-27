using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    public float totalTime, timeLeft;
    public Text text;
    public GameObject enemy;

    private int seconds;
    private string t;
    private Vector3 enemyPos, tPos, bounds;
    private float textPosX, textPosY, radX, radY;

    // Start is called before the first frame update
    void Start()
    {
        bounds = enemy.GetComponent<Renderer>().bounds.size;
        radX = bounds.x;
        radY = bounds.y;
        timeLeft = totalTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        enemyPos = enemy.transform.position;
        textPosX = enemy.transform.position.x + radX;
        textPosY = enemy.transform.position.y + radY;
        tPos = new Vector3(textPosX, textPosY, 0);
        text.transform.position = tPos;

        timeLeft -= Time.deltaTime;
        seconds = Mathf.RoundToInt(timeLeft);


        if (timeLeft <= 0) seconds = 0;
        else seconds = Mathf.RoundToInt(timeLeft);

        t = seconds.ToString();
        // text.text = t;


    }

    //when you evade the enemy for long enough
    public bool outOfTime()
    {
        if (timeLeft <= 0) return true;
        return false;
    }

    //reset timer
    public void resetTimer()
    {
        timeLeft = totalTime;
    }
}
