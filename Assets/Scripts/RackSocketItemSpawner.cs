using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackSocketItemSpawner : MonoBehaviour
{
	public GameObject itemPrefab = null;

	private void Start()
	{
		GameObject.Instantiate(itemPrefab, transform.position, transform.rotation);
		GameObject.Destroy(this);
	}
}
