using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFinish : MonoBehaviour
{
    public GameObject player;
    public PlayerController script;
    public Image screen;
    public Text finishText;
    private bool unlocked = false;
    private int delay = 30;
    private int count = 0;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == player.tag && unlocked) {
            Color fixedColor = screen.color;
            fixedColor.a = 1;
            screen.color = fixedColor;
            screen.CrossFadeAlpha(0f, 0f, true);
            screen.CrossFadeAlpha(1, 3f, false);
            Color fixedColour = finishText.color;
            fixedColour.a = 1;
            finishText.color = fixedColour;
            finishText.CrossFadeAlpha(0f, 0f, true);
            finishText.CrossFadeAlpha(1, 3f, false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == player.tag && unlocked) {
                count += 1;
        }
        if(count >= delay) {
            script.gameFinished = true;
        }
    }

    public void Unlock() {
        unlocked = true;
    }
}
