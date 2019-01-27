using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fade : MonoBehaviour
{
    public GameObject cat;
    public float yPos;
    public Image img;

    private float time, transparency, dT, currA;

    // Start is called before the first frame update
    void Start()
    {

        transparency = img.color.a;
        currA = transparency;
        
    }

    // Update is called once per frame
    void Update()
    {

        dT = transparency / (2 / Time.deltaTime);

        if (cat.transform.position.y >= yPos)
        {
            currA -= dT;
            img.color = new Color(img.color.r, img.color.b, img.color.g, currA);
        }

       
    }
}
