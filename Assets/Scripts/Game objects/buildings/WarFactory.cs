using UnityEngine;
using System.Collections;

public class WarFactory : Building {
	public override void Start()
	{
		base.Start();
		print ("setting to 5");
		gameObject.transform.position = new Vector3(gameObject.transform.position.x, 5, gameObject.transform.position.z);
	}

	public string getName()
	{
		return "factory";
	}
}
