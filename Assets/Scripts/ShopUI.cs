using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
	public GameObject revenueLabel;
	public GameObject customersLabel;
	public GameObject timeLeftLabel;

	private Text[] cachedRevenueTexts;
	private Text[] cachedCustomersTexts;
	private Text[] cachedTimeLeftTexts;
	
	private void Awake()
	{
		cachedRevenueTexts = revenueLabel.GetComponentsInChildren<Text>();
		cachedCustomersTexts = customersLabel.GetComponentsInChildren<Text>();
		cachedTimeLeftTexts = timeLeftLabel.GetComponentsInChildren<Text>();
	}

	private void Update()
	{
		// Revenue
		int dollars = ShopRegistry.instance.grossRevenue / 100;
		int cents = ShopRegistry.instance.grossRevenue % 100;

		string text = string.Format("Gross: ${0}.{1}", dollars, cents);
		foreach (Text cachedText in cachedRevenueTexts)
			cachedText.text = text;

		// Customers
		int happy = ShopRegistry.instance.happyCustomers;
		int disappointed = ShopRegistry.instance.disappointedCustomers;

		text = string.Format("Customers: {0} (Happy) / {1} (Disappointed)", happy, disappointed);
		foreach (Text cachedText in cachedCustomersTexts)
			cachedText.text = text;

		// Time
		int minutes = (int)(ShopRegistry.instance.timeLeft / 60.0f);
		int seconds = (int)(ShopRegistry.instance.timeLeft % 60.0f);

		text = string.Format("Time Left: {0}:{1}", minutes, seconds);
		foreach (Text cachedText in cachedTimeLeftTexts)
			cachedText.text = text;
	}
}
