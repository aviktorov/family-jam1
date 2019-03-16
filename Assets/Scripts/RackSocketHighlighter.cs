using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class GameObjectExtensions
{
	public static void RemoveComponents<T>(this GameObject proxy) where T: Component
	{
		T[] components = proxy.GetComponentsInChildren<T>();
		foreach (T component in components)
			GameObject.Destroy(component);
	}
}

public class RackSocketHighlighter : MonoBehaviour
{
	public Material highlightMaterial = null;

	private GameObject highlightProxy = null;

	private GameObject MakeProxy(GameObject item, Vector3 position, Quaternion orientation)
	{
		GameObject proxy = GameObject.Instantiate(item, position, orientation);

		proxy.RemoveComponents<Collider>();
		proxy.RemoveComponents<Rigidbody>();
		proxy.RemoveComponents<MonoBehaviour>();

		MeshRenderer[] renderers = proxy.GetComponentsInChildren<MeshRenderer>();

		for (int i = 0; i < renderers.Length; i++)
		{
			MeshRenderer renderer = renderers[i];
			renderer.receiveShadows = false;
			renderer.shadowCastingMode = ShadowCastingMode.Off;
			renderer.sharedMaterial = highlightMaterial;
			for (int j = 0; j < renderer.sharedMaterials.Length; j++)
				renderer.sharedMaterials[j] = highlightMaterial;
		}

		return proxy;
	}

	public void SetHighlightItem(GameObject item, Vector3 position, Quaternion orientation)
	{
		if (highlightProxy)
		{
			GameObject.Destroy(highlightProxy);
			highlightProxy = null;
		}

		if (item == null)
			return;

		highlightProxy = MakeProxy(item, position, orientation);
	}
}
