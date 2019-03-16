using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackSocketController : MonoBehaviour
{
	public bool Attached { get; set; }
	public Vector3 AttachedSocketPosition { get; set; }
	public Quaternion AttachedSocketOrientation { get; set; }

	private RackSocketHighlighter cachedHighlighter;

	private void Start()
	{
		cachedHighlighter = GetComponent<RackSocketHighlighter>();
	}

	public void Reset()
	{
		Attached = false;
		cachedHighlighter.SetHighlightItem(null, Vector3.zero, Quaternion.identity);
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.tag != "RackSocket")
			return;

		Attached = true;
		AttachedSocketPosition = collider.bounds.center;
		AttachedSocketOrientation = collider.transform.rotation;
		cachedHighlighter.SetHighlightItem(this.gameObject, AttachedSocketPosition, AttachedSocketOrientation);
	}

	private void OnTriggerExit(Collider collider)
	{
		if (collider.tag != "RackSocket")
			return;

		Reset();
	}
}
