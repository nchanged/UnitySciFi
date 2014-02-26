using UnityEngine;
using System.Collections;
using Pathfinding;

public abstract class Unit : MonoBehaviour, ISelectable, ICommandable
{
	private bool unitSelected = false;

	public Vector3 targetPosition;
	public float speed = 100;                //The AI's speed per second
	protected Seeker seeker;                   //Finds the best path to the targetPosition
	private Path path;                       //The calculated path
	private int currentWaypoint = 0;         //Index of the waypoint we are currently moving towards
	private float nextWaypointDistance = 2;  //Next waypoint needs to be within this distance to consider moving to it

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
