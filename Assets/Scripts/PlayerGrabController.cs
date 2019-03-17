using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabController : MonoBehaviour
{
	public const int gameplayLayerMask = 1 << 9;
	public const int rackSocketsMask = 1 << 10;

	public float grabDistance = 3.0f;
	public Transform cameraTransform;

	private GrabController cachedGrabController;
	private RackSocketHighlighter cachedRackSocketHighlighter;

	public Collider GrabbedCollider { get; private set; }
	public Rigidbody GrabbedBody
	{
		get { return (GrabbedCollider) ? GrabbedCollider.attachedRigidbody : null; }
	}

	public Transform RackSocket { get; private set; }

	private void Awake()
	{
		cachedGrabController = GetComponent<GrabController>();
		cachedRackSocketHighlighter = GetComponent<RackSocketHighlighter>();
	}

	private void LateUpdate()
	{
		Debug.DrawRay(cameraTransform.position, cameraTransform.forward * grabDistance);

		RackSocket = null;
		GrabbedCollider = null;

		RaycastHit hit;
		if (cachedGrabController.GrabbedBody)
		{
			GameObject grabbedObject = cachedGrabController.GrabbedBody.gameObject;

			Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, grabDistance, rackSocketsMask);
			Transform newRackSocket = (hit.collider) ? hit.collider.transform : null;

			if (newRackSocket == null || RackSocket != newRackSocket)
			{
				RackSocket = newRackSocket;
				if (RackSocket)
					cachedRackSocketHighlighter.SetHighlightItem(grabbedObject, RackSocket.position, RackSocket.rotation);
				else
					cachedRackSocketHighlighter.SetHighlightItem(null, Vector3.zero, Quaternion.identity);
			}
		}
		else
		{
			Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, grabDistance, gameplayLayerMask);
			GrabbedCollider = hit.collider;
		}

		if (Input.GetButtonDown("Fire1"))
		{
			cachedRackSocketHighlighter.SetHighlightItem(null, Vector3.zero, Quaternion.identity);
			if (cachedGrabController.GrabbedBody)
				cachedGrabController.Release(RackSocket);
			else if (hit.rigidbody)
			{
				StoreCrateController controller = hit.rigidbody.gameObject.GetComponent<StoreCrateController>();
				if (controller)
				{
					GameObject item = controller.FetchItem();
					if (item)
						cachedGrabController.Grab(item.GetComponent<Rigidbody>());
				}
				else
					cachedGrabController.Grab(hit.rigidbody);
			}
		}
	}
}
