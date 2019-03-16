using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JamSuite;


public class PlayerControllerFirstPerson : MonoBehaviour
{
	public float turnSpeed = 360.0f;
	public float movementSpeed = 10.0f;
	public const float ANGLE_LIMIT = 89.0f;

	private Camera cachedCamera;
	private Transform cachedCameraTransform;
	private CharacterController cachedCharacterController;

	private Vector2 angles;
	private Quaternion initialOrientation;

	private void Awake()
	{
		cachedCamera = GetComponentInChildren<Camera>();
		cachedCameraTransform = cachedCamera.transform;
		cachedCharacterController = GetComponent<CharacterController>();

		initialOrientation = cachedCameraTransform.localRotation;
		angles = Vector2.zero;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		// aim
		Vector2 mouseDelta = new Vector2(
			Input.GetAxis("Mouse X"),
			Input.GetAxis("Mouse Y")
		);

		angles += mouseDelta * turnSpeed * Time.deltaTime;
		angles.y = Mathf.Clamp(angles.y, -ANGLE_LIMIT, ANGLE_LIMIT);

		cachedCameraTransform.localRotation = initialOrientation * Quaternion.Euler(-angles.y, angles.x, 0.0f);

		// movement
		Vector3 movement = new Vector3(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical"),
			0.0f
		);

		movement = cachedCameraTransform.forward * movement.y + cachedCameraTransform.right * movement.x;

		if (movement.sqrMagnitude > 1)
			movement.Normalize();

		movement *= movementSpeed;

		cachedCharacterController.SimpleMove(movement);
	}
}
