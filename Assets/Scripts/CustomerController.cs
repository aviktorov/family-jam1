using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CustomerState
{
	Entered,
	LookingForRack,
	EnRouteToRack,
	LookingForItem,
	EnRouteToItem,
	EnRouteToCashboxLine,
	InCashboxLine,
	Buying,
	Disappointing,
	Leaving
}

public class CustomerController : MonoBehaviour
{
	public float reachDistance = 2.0f;
	public bool happy = true;

	private NavMeshAgent cachedAgent;
	private Transform cachedTransform;

	private GameObject wantedItem = null;
	private CustomerState currentState = CustomerState.Entered;

	private void ProcessEnteredState()
	{
		currentState = CustomerState.LookingForItem;
	}

	private void ProcessLookingForRackState()
	{
		// TODO: get free rack socket for required item
	}

	private void ProcessEnRouteToRackState()
	{
		// TODO: check if target rack socket was reached
	}

	private void ProcessLookingForItemState()
	{
		GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

		// TODO: pick closest?
		wantedItem = items[UnityEngine.Random.Range(0, items.Length)];

		cachedTransform = GetComponent<Transform>();
		cachedAgent = GetComponent<NavMeshAgent>();
		cachedAgent.SetDestination(wantedItem.transform.position);

		currentState = CustomerState.EnRouteToItem;
	}

	private void ProcessEnRouteToItemState()
	{
		if (!wantedItem)
		{
			currentState = CustomerState.LookingForItem;
			return;
		}

		if (cachedAgent.remainingDistance > reachDistance)
			return;

		float distanceToItemSqr = (wantedItem.transform.position - cachedTransform.position).sqrMagnitude;
		if (distanceToItemSqr > reachDistance * reachDistance)
		{
			cachedAgent.SetDestination(wantedItem.transform.position);
			return;
		}

		// TODO: grab item
		currentState = CustomerState.LookingForItem;
	}

	private void ProcessEnRouteToCashboxLineState()
	{
		// TODO: register client in cashbox line
	}

	private void ProcessInCashboxLineState()
	{
		// TODO: slowly proceed to the cashbox then transition to buying state
	}

	private void ProcessBuyingState()
	{
		// TODO: play sfx & particle effect
		// TODO: increase store money
	}

	private void ProcessDisappointingState()
	{
		// TODO: play sfx & particle effect
		currentState = CustomerState.Leaving;
	}

	private void ProcessLeavingState()
	{
		// TODO: go to store exit
	}

	private void Update()
	{
		switch (currentState)
		{
			case CustomerState.Entered: ProcessEnteredState(); break;
			case CustomerState.LookingForRack: ProcessLookingForRackState(); break;
			case CustomerState.EnRouteToRack: ProcessEnRouteToRackState(); break;
			case CustomerState.LookingForItem: ProcessLookingForItemState(); break;
			case CustomerState.EnRouteToItem: ProcessEnRouteToItemState(); break;
			case CustomerState.EnRouteToCashboxLine: ProcessEnRouteToCashboxLineState(); break;
			case CustomerState.InCashboxLine: ProcessInCashboxLineState(); break;
			case CustomerState.Buying: ProcessBuyingState(); break;
			case CustomerState.Disappointing: ProcessDisappointingState(); break;
			case CustomerState.Leaving: ProcessLeavingState(); break;
		}
	}
}
