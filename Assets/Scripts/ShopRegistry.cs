using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRegistry : MonoSingleton<ShopRegistry>
{
	public HashSet<GameObject> items = new HashSet<GameObject>();
	public HashSet<GameObject> racks = new HashSet<GameObject>();

	private void TryAdd(Collider collider)
	{
		if (collider.tag == "Item")
			items.Add(collider.gameObject);

		if (collider.tag == "Rack")
			racks.Add(collider.gameObject);
	}

	private void TryRemove(Collider collider)
	{
		if (collider.tag == "Item")
			items.Remove(collider.gameObject);

		if (collider.tag == "Rack")
			racks.Remove(collider.gameObject);
	}

	private void Start()
	{
		BoxCollider cachedCollider = GetComponent<BoxCollider>();

		Collider[] possibleObjects = Physics.OverlapBox(cachedCollider.center, cachedCollider.size * 0.5f, transform.rotation);
		for (int i = 0; i < possibleObjects.Length; i++)
		{
			TryAdd(possibleObjects[i]);
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		TryAdd(collider);
	}

	private void OnTriggerExit(Collider collider)
	{
		TryRemove(collider);
	}
}
