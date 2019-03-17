using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaterials : MonoBehaviour
{
	public Material[] materials = null;

	private void Start()
	{
		if (materials != null && materials.Length > 0)
		{
			Material material = materials[UnityEngine.Random.Range(0, materials.Length)];
			gameObject.SetMaterial(material);
		}
		Destroy(this);
	}
}
