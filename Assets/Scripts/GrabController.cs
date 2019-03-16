using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
	public Transform grabSocket;
	public float grabDistance = 1.0f;

	public float breakForce = 5000.0f;
	public float breakTorque = 5000.0f;

	private FixedJoint grabJoint;
	private RackSocketHighlighter grabRackHighlighter;

	public Rigidbody GrabbedBody
	{
		get { return (grabJoint) ? grabJoint.connectedBody : null; }
	}

	public FixedJoint Grab(Rigidbody body)
	{
		if (!body)
			return null;

		if (!grabJoint)
		{
			grabJoint = grabSocket.gameObject.AddComponent<FixedJoint>();
			grabJoint.anchor = grabSocket.localPosition;
			grabJoint.breakForce = breakForce;
			grabJoint.breakTorque = breakTorque;
		}

		body.transform.position = grabSocket.position;

		grabJoint.connectedBody = body;

		return grabJoint;
	}

	public void Release(Transform socketTransform = null)
	{
		if (!grabJoint)
			return;

		Rigidbody body = grabJoint.connectedBody;
		if (!body)
			return;

		GameObject.Destroy(grabJoint);
		grabJoint = null;

		if (socketTransform)
		{
			body.transform.position = socketTransform.position;
			body.transform.rotation = socketTransform.rotation;
		}
	}
}
