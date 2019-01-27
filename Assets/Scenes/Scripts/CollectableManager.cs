using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
	public GameObject Unlockable;

	private List<GameObject> children = new List<GameObject>();
	private int collected = 0;
    
    void Awake()
    {
		foreach (Transform child in transform)
		{
			children.Add(child.gameObject);
		}
	}

    void Update()
    {
		collected = 0;
        foreach (var child in children)
		{
			if (!child.activeSelf)
				collected++;
		}
		if (collected >= children.Count)
		{
			AllCollected();
		}
    }

	private void AllCollected()
	{
		if (Unlockable)
		{
			Unlockable.SendMessage("Unlock");
		}

		gameObject.SetActive(false);
	}
}
