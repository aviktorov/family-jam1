using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRegistry : MonoSingleton<ShopRegistry>
{
	public int grossRevenue = 0;
	public int happyCustomers = 0;
	public int disappointedCustomers = 0;
	public float timeLeft = 601; // 10 minutes

	public GameObject[] Items
	{
		get { return cachedItems; }
	}

	public GameObject[] Racks
	{
		get { return cachedRacks; }
	}

	private GameObject[] cachedItems = null;
	private GameObject[] cachedRacks = null;

	private HashSet<GameObject> items = new HashSet<GameObject>();
	private HashSet<GameObject> racks = new HashSet<GameObject>();

	private void TryAdd(Collider collider)
	{
		if (collider.tag == "Item")
		{
			items.Add(collider.attachedRigidbody.gameObject);

			cachedItems = new GameObject[items.Count];
			items.CopyTo(cachedItems);
		}

		if (collider.tag == "Rack")
		{
			racks.Add(collider.attachedRigidbody.gameObject);

			cachedRacks = new GameObject[racks.Count];
			racks.CopyTo(cachedRacks);
		}
	}

	private void TryRemove(Collider collider)
	{
		if (collider.tag == "Item")
		{
			items.Remove(collider.attachedRigidbody.gameObject);

			if (items.Count > 0)
			{
				cachedItems = new GameObject[items.Count];
				items.CopyTo(cachedItems);
			}
			else
				cachedItems = null;
		}

		if (collider.tag == "Rack")
		{
			racks.Remove(collider.attachedRigidbody.gameObject);

			if (racks.Count > 0)
			{
				cachedRacks = new GameObject[racks.Count];
				racks.CopyTo(cachedRacks);
			}
			else
				cachedRacks = null;
		}
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

	private void Update()
	{
		timeLeft = Mathf.Max(0.0f, timeLeft - Time.deltaTime);
	}

	private void OnTriggerEnter(Collider collider)
	{
		StartCoroutine("TryAdd", collider);
	}

	private void OnTriggerExit(Collider collider)
	{
		StartCoroutine("TryRemove", collider);
	}
}
