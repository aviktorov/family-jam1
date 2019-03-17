using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RackSocketHighlighter : MonoBehaviour
{
	public Material highlightMaterial = null;

	private GameObject highlightProxy = null;

	public void SetHighlightItem(GameObject item, Vector3 position, Quaternion orientation)
	{
		if (highlightProxy)
		{
			GameObject.Destroy(highlightProxy);
			highlightProxy = null;
		}

		if (item == null)
			return;

		highlightProxy = item.MakeProxy(highlightMaterial, position, orientation);
	}
}
