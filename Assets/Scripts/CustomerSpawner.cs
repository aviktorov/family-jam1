using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
	public GameObject customerPrefab = null;

	[Range(0.0f, 60.0f)]
	public float spawnIntervalMin = 4.0f;

	[Range(0.0f, 60.0f)]
	public float spawnIntervalMax = 15.0f;

	private float countdown;

	private void Start()
	{
		countdown = 0.0f;
	}

	private void Update()
	{
		countdown -= Time.deltaTime;

		if (countdown > 0.0f)
			return;

		float newCountdown = UnityEngine.Random.Range(spawnIntervalMin, spawnIntervalMax);
		while (countdown < 0.0f)
			countdown += newCountdown;

		GameObject.Instantiate(customerPrefab, transform.position, transform.rotation);
	}
}
