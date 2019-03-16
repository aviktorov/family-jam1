using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemRegistry : MonoSingleton<ShopItemRegistry>
{
	public HashSet<GameObject> items = new HashSet<GameObject>();

	private void Awake()
	{
		BoxCollider cachedCollider = GetComponent<BoxCollider>();

		Collider[] possibleItems = Physics.OverlapBox(cachedCollider.center, cachedCollider.size * 0.5f, transform.rotation);
		for (int i = 0; i < possibleItems.Length; i++)
		{
			if (possibleItems[i].tag != "Item")
				continue;

			items.Add(possibleItems[i].gameObject);
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.tag != "Item")
			return;

		items.Add(collider.gameObject);
	}

	private void OnTriggerExit(Collider collider)
	{
		if (collider.tag != "Item")
			return;

		items.Remove(collider.gameObject);
	}
}
