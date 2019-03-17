using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackSocketItemSpawner : MonoBehaviour
{
	public float probability = 0.25f;
	public GameObject[] itemPrefabs = null;

	private void Awake()
	{
		bool shouldSpawn = (UnityEngine.Random.Range(0.0f, 1.0f) < probability);
		if (shouldSpawn && itemPrefabs != null && itemPrefabs.Length > 0)
		{
			GameObject prefab = itemPrefabs[UnityEngine.Random.Range(0, itemPrefabs.Length)];
			if (prefab)
				GameObject.Instantiate(prefab, transform.position, transform.rotation);
		}
		GameObject.Destroy(this);
	}
}
