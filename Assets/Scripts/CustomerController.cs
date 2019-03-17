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

	private NavMeshAgent cachedAgent;
	private Transform cachedTransform;

	private GameObject wantedItem = null;
	private GameObject wantedReachTarget = null;

	private GrabController cachedGrabController;
	private CustomerState currentState = CustomerState.Entered;
	private bool boughtItem = false;
	private bool happy = true;

	private void Awake()
	{
		cachedGrabController = GetComponent<GrabController>();
		cachedTransform = GetComponent<Transform>();
		cachedAgent = GetComponent<NavMeshAgent>();
	}

	private void SetRandomReachTarget(string tag)
	{
		GameObject[] items = GameObject.FindGameObjectsWithTag(tag);

		if (items == null || items.Length == 0)
		{
			wantedReachTarget = null;
			return;
		}

		// TODO: pick closest?
		wantedReachTarget = items[UnityEngine.Random.Range(0, items.Length)];

		cachedAgent.SetDestination(wantedReachTarget.transform.position);
	}

	private void SetRandomRegistryTarget(HashSet<GameObject> targets)
	{
		if (targets == null || targets.Count == 0)
		{
			wantedReachTarget = null;
			return;
		}

		// TODO: remove later
		GameObject[] targetsArray = new GameObject[targets.Count];
		targets.CopyTo(targetsArray);

		wantedReachTarget = targetsArray[UnityEngine.Random.Range(0, targetsArray.Length)];
		cachedAgent.SetDestination(wantedReachTarget.transform.position);
	}

	private bool HandlePathProgress()
	{
		if (cachedAgent.remainingDistance > reachDistance)
			return true;

		float distanceToItemSqr = (wantedReachTarget.transform.position - cachedTransform.position).sqrMagnitude;
		if (distanceToItemSqr > reachDistance * reachDistance)
		{
			cachedAgent.SetDestination(wantedReachTarget.transform.position);
			return true;
		}

		return false;
	}

	private bool HandleItemLoss()
	{
		if (happy && !cachedGrabController.GrabbedBody)
		{
			if (wantedItem)
			{
				currentState = CustomerState.EnRouteToItem;
				cachedAgent.SetDestination(wantedItem.transform.position);
			}
			else
				currentState = CustomerState.LookingForItem;

			return true;
		}

		return false;
	}

	private void ProcessEnteredState()
	{
		currentState = CustomerState.LookingForRack;
	}

	private void ProcessLookingForRackState()
	{
		SetRandomRegistryTarget(ShopRegistry.instance.racks);
		currentState = CustomerState.EnRouteToRack;
	}

	private void ProcessEnRouteToRackState()
	{
		if (HandlePathProgress())
			return;

		currentState = CustomerState.LookingForItem;
	}

	private void ProcessLookingForItemState()
	{
		SetRandomRegistryTarget(ShopRegistry.instance.items);
		wantedItem = wantedReachTarget;

		currentState = (wantedReachTarget) ? CustomerState.EnRouteToItem : CustomerState.Disappointing;
	}

	private void ProcessEnRouteToItemState()
	{
		if (!wantedItem)
		{
			currentState = CustomerState.LookingForItem;
			return;
		}

		if (HandlePathProgress())
			return;

		// Grab new item and release old
		cachedGrabController.Release();
		cachedGrabController.Grab(wantedItem.GetComponent<Rigidbody>());

		currentState = (boughtItem) ? CustomerState.LookingForExit : CustomerState.LookingForCashboxLine;
	}

	private void ProcessLookingForCashboxLineState()
	{
		SetRandomReachTarget("CashboxLine");
		currentState = CustomerState.EnRouteToCashboxLine;
	}

	private void ProcessEnRouteToCashboxLineState()
	{
		// TODO: implement complex line handling logic
		if (HandleItemLoss())
			return;

		if (HandlePathProgress())
			return;

		currentState = CustomerState.Buying;
	}

	private void ProcessInCashboxLineState()
	{
		// TODO: slowly proceed to the cashbox then transition to buying state
	}

	private void ProcessLookingForExitState()
	{
		SetRandomReachTarget("Exit");
		currentState = CustomerState.EnRouteToExit;
	}

	private void ProcessEnRouteToExitState()
	{
		if (HandleItemLoss())
			return;

		if (HandlePathProgress())
			return;

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
		wantedItem = null;
		cachedGrabController.Release();
	}

	private void ProcessLeavingState()
	{
		// TODO: play sfx & particle effect
		if (cachedGrabController.GrabbedBody)
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
