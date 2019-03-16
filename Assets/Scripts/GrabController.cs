using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
	public Transform cameraTransform;
	public Transform grabSocket;
	public float grabDistance = 1.0f;

	public float breakForce = 100.0f;
	public float breakTorque = 100.0f;

	private FixedJoint grabJoint;
	private RackSocketController grabRackSocket;
	private RackSocketHighlighter grabRackHighlighter;

	public Collider GrabCollider { get; private set; }

	private FixedJoint FetchJoint(Rigidbody body)
	{
		if (!body)
			return null;

		if (!grabJoint)
		{
			grabJoint = grabSocket.gameObject.AddComponent<FixedJoint>();
			grabJoint.anchor = grabSocket.localPosition;
			// grabJoint.breakForce = breakForce;
			// grabJoint.breakTorque = breakTorque;
		}

		body.transform.position = grabSocket.position;

		grabRackSocket = body.gameObject.GetComponent<RackSocketController>();
		grabJoint.connectedBody = body;

		return grabJoint;
	}

	private void ReleaseJoint()
	{
		Rigidbody body = grabJoint.connectedBody;
		if (!body)
			return;

		GameObject.Destroy(grabJoint);
		grabJoint = null;

		if (grabRackSocket)
		{
			if (grabRackSocket.Attached)
			{
				body.transform.position = grabRackSocket.AttachedSocketPosition;
				body.transform.rotation = grabRackSocket.AttachedSocketOrientation;
			}
			grabRackSocket.Reset();
		}
	}

	void LateUpdate()
	{
		Debug.DrawRay(cameraTransform.position, cameraTransform.forward * grabDistance);

		RaycastHit hit;
		Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, grabDistance);

		GrabCollider = hit.collider;

		if (Input.GetButtonDown("Fire1"))
		{
			if (grabJoint)
				ReleaseJoint();
			else
				FetchJoint(hit.rigidbody);
		}
	}
}
