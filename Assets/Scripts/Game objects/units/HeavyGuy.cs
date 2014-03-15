using UnityEngine;
using System.Collections;

public class HeavyGuy : Unit {
	public override void Start()
	{
		this.SelectAnimations = new string[] { "shoot_01", "shoot_02", "shoot_03", "emo_1" };		
		this.DefaultAnimation = "idle_1";
		base.Start();
		NavMeshAgent agent = (NavMeshAgent)gameObject.GetComponent<NavMeshAgent>();
		agent.angularSpeed = 200;
		agent.speed = 10;
		agent.acceleration = 8;
		
		this.animation["walk"].speed = 3f;
		this.animation["emo_1"].speed = 2f;
	}
}
