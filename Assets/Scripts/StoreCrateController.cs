using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreCrateController : MonoBehaviour
{
	public GameObject itemPrefab = null;
	public Transform spawnSocket = null;
	public Transform infoSocket = null;
	public Material highlightMaterial = null;
	public float proxyTurnSpeed = 5.0f;

	private GameObject proxy;

	private void Start()
	{
		if (!itemPrefab || !infoSocket)
			return;

		proxy = itemPrefab.MakeProxy(highlightMaterial, infoSocket.position, infoSocket.rotation);
	}

	private void Update()
	{
		if (!proxy)
			return;

		proxy.transform.Rotate(0.0f, proxyTurnSpeed * Time.deltaTime, 0.0f);
	}

	public GameObject FetchItem()
	{
		if (!itemPrefab || !spawnSocket)
			return null;

		return GameObject.Instantiate(itemPrefab, spawnSocket.position, spawnSocket.rotation);
	}
}
