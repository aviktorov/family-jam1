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
	LookingForCashboxLine,
	EnRouteToCashboxLine,
	InCashboxLine,
	LookingForExit,
	EnRouteToExit,
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
	private GameObject wantedCashboxLine = null;
	private GameObject wantedExit = null;

	private GrabController cachedGrabController;
	private CustomerState currentState = CustomerState.Entered;
	private bool boughtItem = false;

	private void Awake()
	{
		cachedGrabController = GetComponent<GrabController>();
	}

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

		// Grab new item and release old
		cachedGrabController.Release();
		cachedGrabController.Grab(wantedItem.GetComponent<Rigidbody>());

		currentState = (boughtItem) ? CustomerState.LookingForExit : CustomerState.LookingForCashboxLine;
	}

	private void ProcessLookingForCashboxLineState()
	{
		// Pick desired cashbox line
		GameObject[] lines = GameObject.FindGameObjectsWithTag("CashboxLine");

		// TODO: pick closest?
		wantedCashboxLine = lines[UnityEngine.Random.Range(0, lines.Length)];

		currentState = CustomerState.EnRouteToCashboxLine;
	}

	private void ProcessEnRouteToCashboxLineState()
	{
		// TODO: implement complex line handling logic

		if (!wantedCashboxLine)
		{
			currentState = CustomerState.Leaving;
			return;
		}

		if (!cachedGrabController.GrabbedBody)
		{
			if (wantedItem)
			{
				currentState = CustomerState.EnRouteToItem;
				cachedAgent.SetDestination(wantedItem.transform.position);
			}
			else
				currentState = CustomerState.LookingForItem;

			return;
		}

		if (cachedAgent.remainingDistance > reachDistance)
			return;

		float distanceToItemSqr = (wantedCashboxLine.transform.position - cachedTransform.position).sqrMagnitude;
		if (distanceToItemSqr > reachDistance * reachDistance)
		{
			cachedAgent.SetDestination(wantedCashboxLine.transform.position);
			return;
		}

		currentState = CustomerState.Buying;
	}

	private void ProcessInCashboxLineState()
	{
		// TODO: slowly proceed to the cashbox then transition to buying state
	}

	private void ProcessLookingForExitState()
	{
		GameObject[] exits = GameObject.FindGameObjectsWithTag("Exit");

		// TODO: pick closest?
		wantedExit = exits[UnityEngine.Random.Range(0, exits.Length)];

		currentState = CustomerState.EnRouteToExit;
	}

	private void ProcessEnRouteToExitState()
	{
		if (!wantedExit)
		{
			currentState = CustomerState.LookingForExit;
			return;
		}

		if (happy && !cachedGrabController.GrabbedBody)
		{
			if (wantedItem)
			{
				currentState = CustomerState.EnRouteToItem;
				cachedAgent.SetDestination(wantedItem.transform.position);
			}
			else
				currentState = CustomerState.LookingForItem;

			return;
		}

		if (cachedAgent.remainingDistance > reachDistance)
			return;

		float distanceToItemSqr = (wantedExit.transform.position - cachedTransform.position).sqrMagnitude;
		if (distanceToItemSqr > reachDistance * reachDistance)
		{
			cachedAgent.SetDestination(wantedExit.transform.position);
			return;
		}

		currentState = CustomerState.Leaving;
	}

	private void ProcessBuyingState()
	{
		// TODO: play sfx & particle effect
		// TODO: increase store money
		Debug.Log("Bought an item!");
		boughtItem = true;

		currentState = CustomerState.LookingForExit;
	}

	private void ProcessDisappointingState()
	{
		// TODO: play sfx & particle effect

		currentState = CustomerState.LookingForExit;
		happy = false;
	}

	private void ProcessLeavingState()
	{
		// TODO: play sfx & particle effect
		GameObject.Destroy(cachedGrabController.GrabbedBody.gameObject);
		GameObject.Destroy(gameObject);
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
			case CustomerState.LookingForCashboxLine: ProcessLookingForCashboxLineState(); break;
			case CustomerState.EnRouteToCashboxLine: ProcessEnRouteToCashboxLineState(); break;
			case CustomerState.InCashboxLine: ProcessInCashboxLineState(); break;
			case CustomerState.LookingForExit: ProcessLookingForExitState(); break;
			case CustomerState.EnRouteToExit: ProcessEnRouteToExitState(); break;
			case CustomerState.Buying: ProcessBuyingState(); break;
			case CustomerState.Disappointing: ProcessDisappointingState(); break;
			case CustomerState.Leaving: ProcessLeavingState(); break;
		}
	}
}
