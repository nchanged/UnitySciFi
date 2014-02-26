using UnityEngine;
using System.Collections;

public class HeavyGuy : Unit {
	void Start()
	{
		this.DefaultAnimation = "idle_1";
		base.Start();
		NavMeshAgent agent = (NavMeshAgent)gameObject.GetComponent<NavMeshAgent>();
		agent.angularSpeed = 200;
		agent.speed = 10;
		agent.acceleration = 8;

		this.animation["walk"].speed = 3f;
	}
}
