using UnityEngine;
using System.Collections;

public class WarFactory : Building {

	private float minOpacity = 0.27f;
	private float maxOpacity = 0.6f;
	private bool increaseOpacity = true;

	public override void Start()
	{
		base.Start();
		gameObject.transform.position = new Vector3(gameObject.transform.position.x, 5, gameObject.transform.position.z);
	}

	public override void Update()
	{
		Material glowMaterial = renderer.materials[1];
		float currentOpacity = glowMaterial.color.a;
		if(currentOpacity < maxOpacity && increaseOpacity)
		{
			Color newColor = glowMaterial.color;
			newColor.a = currentOpacity + 0.01f;
			glowMaterial.color = newColor;
			if(newColor.a >= maxOpacity)
			{
				increaseOpacity = false;
			}
		}
		if(currentOpacity > minOpacity && !increaseOpacity)
		{
			Color newColor = glowMaterial.color;
			newColor.a = currentOpacity - 0.01f;
			glowMaterial.color = newColor;
			if(newColor.a <= minOpacity)
			{
				increaseOpacity = true;
			}
		}
	}

	public string getName()
	{
		return "factory";
	}
}
