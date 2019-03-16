using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorHighlighter : MonoBehaviour
{
	public PlayerGrabController grabController = null;

	public Color highlightColor = Color.red;
	public float smoothness = 20.0f;

	private Image cachedImage = null;
	private Color cachedColor;
	private Color targetColor;

	private void Awake()
	{
		cachedImage = gameObject.GetComponent<Image>();
		cachedColor = cachedImage.color;
	}

	private void Update()
	{
		Color targetColor = cachedColor;
		Collider collider = (grabController) ? grabController.GrabCollider : null;

		if (collider && collider.tag == "Item")
			targetColor = highlightColor;

		cachedImage.color = Color.Lerp(cachedImage.color, targetColor, smoothness * Time.deltaTime);
	}
}
