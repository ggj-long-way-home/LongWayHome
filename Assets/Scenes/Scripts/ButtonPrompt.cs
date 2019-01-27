using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPrompt : MonoBehaviour
{
	public Font font;
	public string text;
	public GameObject player;
	[Range(0f, 30f)]
	public float fadeDuration = 5.0f;

	private SpriteRenderer render;
	private float fadeTimer;
	private float maxOpacity = 0.8f;
	private bool playerInRange = false;

	// Start is called before the first frame update
	void Start()
    {
		render = GetComponent<SpriteRenderer>();
		render.color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
		float shiftTowards = playerInRange ? maxOpacity : 0f;
		render.color = new Color(1f, 1f, 1f, Mathf.Clamp(render.color.a + (maxOpacity / fadeDuration) * (playerInRange ? 1 : -1), 0f, maxOpacity) );
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == player.gameObject)
			playerInRange = true;
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player.gameObject)
			playerInRange = false;
	}
}
