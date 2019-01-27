using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rextFade : MonoBehaviour
{
    public float time;
    public Text txt;

    private float currTime, halfTime, transparency, dT, currA;

    // Start is called before the first frame update
    void Start()
    {
        currTime = time;
        halfTime = time / 2;
        transparency = txt.color.a;
        currA = transparency;

    }

    // Update is called once per frame
    void Update()
    {
        print(dT);
        print(Time.deltaTime);

        dT = transparency / (halfTime / Time.deltaTime);

        currTime -= Time.deltaTime;

        if (currTime <= halfTime)
        {
            currA -= dT;
            txt.color = new Color(txt.color.r, txt.color.b, txt.color.g, currA);
        }

        print(currA);
    }
}
