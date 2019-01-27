using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyUnlockable : MonoBehaviour, IUnlockable
{
    public void Unlock()
    {
		Destroy(transform.root.gameObject);
	}
}
