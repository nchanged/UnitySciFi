using UnityEngine;
using System.Collections;

public class TextureSwitcher : MonoBehaviour {

	public static Material AddSecondaryMaterial(GameObject target, string materialPath)
	{
		Material baseMaterial = target.renderer.materials[0];
		Material secondaryMaterial = Resources.Load(materialPath, typeof(Material)) as Material;
		Material[] materialsArray = new Material[2];
		materialsArray[0] = baseMaterial;
		materialsArray[1] = secondaryMaterial;
		target.renderer.materials = materialsArray;
		return target.renderer.materials[1];
	}
}
