using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabController : MonoBehaviour
{
	public Transform cameraTransform;

	private GrabController cachedGrabController;

	public Collider GrabCollider { get; private set; }

	private void Awake()
	{
		cachedGrabController = GetComponent<GrabController>();
	}

	private void LateUpdate()
	{
		Debug.DrawRay(cameraTransform.position, cameraTransform.forward * cachedGrabController.grabDistance);

		RaycastHit hit;
		Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, cachedGrabController.grabDistance);

		GrabCollider = hit.collider;

		if (Input.GetButtonDown("Fire1"))
		{
			if (cachedGrabController.GrabbedBody)
				cachedGrabController.Release();
			else
				cachedGrabController.Grab(hit.rigidbody);
		}
	}
}
