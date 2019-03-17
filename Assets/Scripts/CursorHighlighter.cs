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
		Rigidbody body = (grabController) ? grabController.GrabbedBody : null;
		Color targetColor = (body) ? highlightColor : cachedColor;

		cachedImage.color = Color.Lerp(cachedImage.color, targetColor, smoothness * Time.deltaTime);
	}
}
