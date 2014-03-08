using UnityEngine;
using System.Collections;

public class Chicken : Unit {
	void Start()
	{
		this.SelectAnimations = new string[] { "power_on" };	
		this.DefaultAnimation = "shoot1";
		base.Start();
		NavMeshAgent agent = (NavMeshAgent)gameObject.GetComponent<NavMeshAgent>();
		agent.angularSpeed = 250;
		agent.speed = 15;
		agent.acceleration = 15;

		this.animation["walk"].speed = 4f;
	}
}
