using UnityEngine;
using System.Collections;

public class Probe : Unit {

	void Start()
	{
		this.SelectAnimations = new string[] { "Probe_Idle" };		
		this.DefaultAnimation = "Probe_Idle";
		base.Start();
		NavMeshAgent agent = (NavMeshAgent)gameObject.GetComponent<NavMeshAgent>();
		agent.angularSpeed = 200;
		agent.speed = 10;
		agent.acceleration = 8;
		
		//this.animation["walk"].speed = 3f;
	}
}
