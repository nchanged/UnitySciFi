using UnityEngine;
using System.Collections;

public class PulseGlow : MonoBehaviour {
	private float minOpacity = 0.1f;
	private float maxOpacity = 0.6f;
	private float pulseSpeed = 0.5f;
	private bool increaseOpacity = true;
	private Material glowMaterial = null;

	void Start()
	{
		glowMaterial = TextureSwitcher.AddSecondaryMaterial(gameObject, "DynamicMaterials/Glow");
	}
	
	void Update()
	{
		if(glowMaterial != null)
		{
			changeOffset ();
			changeOpacity ();
		}
	}

	private void changeOffset()
	{
		float currentOffset = glowMaterial.mainTextureOffset.x;
		if(currentOffset < 10)
		{
			glowMaterial.mainTextureOffset = new Vector2(currentOffset + 1 * Time.deltaTime, 0);
		}
		else 
		{
			glowMaterial.mainTextureOffset = Vector2.zero;
		}
	}

	private void changeOpacity()
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
		else if(currentOpacity > minOpacity && !increaseOpacity)
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
