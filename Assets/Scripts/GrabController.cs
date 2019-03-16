using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
	public Transform grabSocket;
	public float grabDistance = 1.0f;

	public float breakForce = 100.0f;
	public float breakTorque = 100.0f;

	private FixedJoint grabJoint;
	private RackSocketController grabRackSocket;
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

		grabRackSocket = body.gameObject.GetComponent<RackSocketController>();
		grabJoint.connectedBody = body;

		return grabJoint;
	}

	public void Release()
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
}
