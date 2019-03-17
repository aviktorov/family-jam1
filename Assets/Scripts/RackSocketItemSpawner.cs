using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackSocketItemSpawner : MonoBehaviour
{
	public GameObject[] itemPrefabs = null;

	private void Awake()
	{
		if (itemPrefabs != null && itemPrefabs.Length > 0)
		{
			GameObject prefab = itemPrefabs[UnityEngine.Random.Range(0, itemPrefabs.Length)];
			if (prefab)
				GameObject.Instantiate(prefab, transform.position, transform.rotation);
		}
		GameObject.Destroy(this);
	}
}
