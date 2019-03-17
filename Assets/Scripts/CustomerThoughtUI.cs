using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerThoughtUI : MonoBehaviour
{
	public Sprite enteredSprite = null;
	public Sprite lookingForRackSprite = null;
	public Sprite enRouteToRackSprite = null;
	public Sprite lookingForItemSprite = null;
	public Sprite enRouteToItemSprite = null;
	public Sprite lookingForCashboxLineSprite = null;
	public Sprite enRouteToCashboxLineSprite = null;
	public Sprite inCashboxLineSprite = null;
	public Sprite lookingForExitSprite = null;
	public Sprite enRouteToExitSprite = null;
	public Sprite buyingSprite = null;
	public Sprite disappointingSprite = null;
	public Sprite leavingSprite = null;

	public CustomerController cachedController;

	private SpriteRenderer cachedRenderer;

	private void Awake()
	{
		cachedRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (!cachedController)
			return;

		if (!cachedController.Happy)
		{
			cachedRenderer.sprite = disappointingSprite;
			return;
		}

		switch (cachedController.State)
		{
			case CustomerState.Entered: cachedRenderer.sprite = enteredSprite; break;
			case CustomerState.LookingForRack: cachedRenderer.sprite = lookingForRackSprite; break;
			case CustomerState.EnRouteToRack: cachedRenderer.sprite = enRouteToRackSprite; break;
			case CustomerState.LookingForItem: cachedRenderer.sprite = lookingForItemSprite; break;
			case CustomerState.EnRouteToItem: cachedRenderer.sprite = enRouteToItemSprite; break;
			case CustomerState.LookingForCashboxLine: cachedRenderer.sprite = lookingForCashboxLineSprite; break;
			case CustomerState.EnRouteToCashboxLine: cachedRenderer.sprite = enRouteToCashboxLineSprite; break;
			case CustomerState.InCashboxLine: cachedRenderer.sprite = inCashboxLineSprite; break;
			case CustomerState.LookingForExit: cachedRenderer.sprite = lookingForExitSprite; break;
			case CustomerState.EnRouteToExit: cachedRenderer.sprite = enRouteToExitSprite; break;
			case CustomerState.Buying: cachedRenderer.sprite = buyingSprite; break;
			case CustomerState.Disappointing: cachedRenderer.sprite = disappointingSprite; break;
			case CustomerState.Leaving: cachedRenderer.sprite = leavingSprite; break;
		}
	}
}
