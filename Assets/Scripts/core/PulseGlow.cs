﻿using UnityEngine;
using System.Collections;

public class PulseGlow : MonoBehaviour {
	private float minOpacity = 0.1f;
	private float maxOpacity = 0.6f;
	private float pulseSpeed = 0.5f;
	private bool increaseOpacity = true;
	private Material glowMaterial = null;

	void Start()
	{
		int length = renderer.materials.Length;
		Material[] materialsArray = new Material[length + 1];
		for(int i = 0; i < renderer.materials.Length; i++)
		{
			materialsArray[i] = renderer.materials[i];
		}
		materialsArray[length] = Resources.Load("DynamicMaterials/Glow", typeof(Material)) as Material;
		renderer.materials = materialsArray;
		glowMaterial = renderer.materials[length];
	}
	
	void Update()
	{
		if(glowMaterial != null)
		{
			float currentOpacity = glowMaterial.color.a;

			if(currentOpacity < maxOpacity && increaseOpacity)
			{
				Color newColor = glowMaterial.color;
				newColor.a = currentOpacity + pulseSpeed * Time.deltaTime;
				glowMaterial.color = newColor;
				if(newColor.a >= maxOpacity)
				{
					increaseOpacity = false;
				}
			}
			if(currentOpacity > minOpacity && !increaseOpacity)
			{
				Color newColor = glowMaterial.color;
				newColor.a = currentOpacity - pulseSpeed * Time.deltaTime;
				glowMaterial.color = newColor;
				if(newColor.a <= minOpacity)
				{
					increaseOpacity = true;
				}
			}
		}
	}
}
