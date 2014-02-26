using UnityEngine;
using System.Collections;

public abstract class Unit : MonoBehaviour, ISelectable, ICommandable
{
	private bool unitSelected = false;
	public string DefaultAnimation {get;set;}
	public NavMeshAgent PathfindingAgent;
	private Vector3 destinationPosition;
	private bool reachedDestination = true;

	public void Start ()
	{
		this.animation.wrapMode = WrapMode.Loop;		
		this.animation.Play();
		this.animation.CrossFade(this.DefaultAnimation);
		PathfindingAgent = (NavMeshAgent)gameObject.AddComponent("NavMeshAgent");
		destinationPosition = gameObject.transform.position;
	}

	public void Update()
	{
		if(!reachedDestination)
		{
			float distanceLeft = Vector3.Distance(gameObject.transform.position, destinationPosition);
			if (distanceLeft <= 1)
			{
				this.animation.CrossFade(this.DefaultAnimation);
				PathfindingAgent.Stop ();
				reachedDestination = true;
			}
		}
	}

	public void OnMoveCommand(Vector3 destination)
	{
		destination = new Vector3(destination.x,0,destination.z);
		destinationPosition = destination;
		PathfindingAgent.SetDestination(destination);
		this.animation.CrossFade("walk");
		reachedDestination = false;
	}

	public void OnSelect()
	{
		unitSelected = true;
	}
	public void OnDeselect()
	{
		unitSelected = false;
	}

	public bool IsSelected
	{
		get{return unitSelected;}
		set{unitSelected = value;}
	}
}
