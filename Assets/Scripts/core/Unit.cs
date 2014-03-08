using UnityEngine;
using System.Collections;

public abstract class Unit : MonoBehaviour, ISelectable, ICommandable
{
	private bool unitSelected = false;
	public string DefaultAnimation {get;set;}
	private NavMeshAgent PathfindingAgent;
	private Vector3 destinationPosition;
	private bool reachedDestination = true;

	public void Start ()
	{
		this.animation.wrapMode = WrapMode.Loop;		
		this.animation.Play();
		this.animation.CrossFade(this.DefaultAnimation);
		PathfindingAgent = (NavMeshAgent)gameObject.AddComponent("NavMeshAgent");
		destinationPosition = gameObject.transform.position;

		Rigidbody rigidbody = (Rigidbody)gameObject.AddComponent("Rigidbody");
		rigidbody.mass = 50f;
		rigidbody.drag = 50f;
		rigidbody.angularDrag = 50f;
		rigidbody.useGravity = true;
		rigidbody.freezeRotation = true;
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
