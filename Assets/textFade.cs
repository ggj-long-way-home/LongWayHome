using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textFade : MonoBehaviour
{
    public GameObject cat;
    public Text txt;
    public float distance, start;

    private float time, transparency, dT, currA, initPos;
    private bool st;

    // Start is called before the first frame update
    void Start()
    {
        initPos = cat.transform.position.y;
        txt.color = new Color(txt.color.r, txt.color.b, txt.color.g, 0);
        st = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(cat.transform.position.y >= initPos + start)
        {
            if (st)
            {
                txt.color = new Color(txt.color.r, txt.color.b, txt.color.g, 1);
                transparency = 1;
                currA = 1;
                st = false;
            }
        }

        dT = transparency / (2 / Time.deltaTime);

        if (cat.transform.position.y >= initPos + start + distance)
        {
            currA -= dT;
            txt.color = new Color(txt.color.r, txt.color.b, txt.color.g, currA);
        }


    }
}
