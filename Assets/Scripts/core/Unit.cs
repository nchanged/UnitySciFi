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
	
	public void Start ()
	{
		seeker = gameObject.AddComponent("Seeker") as Seeker;
		gameObject.AddComponent("SimpleSmoothModifier");


		this.animation.wrapMode = WrapMode.Loop;
		
		this.animation.Play();
		if ( this.name == "heavy-bot") {
			this.animation.CrossFade("emo_2");
		}



	}

	public void OnMoveCommand(Vector3 destination)
	{
		destination = new Vector3(destination.x,0,destination.z);
		
		//Start a new path to the targetPosition, return the result to the OnPathComplete function

		seeker.StartPath (transform.position,destination, OnPathComplete);

		this.animation.Play();
		this.animation.CrossFade ("run");
		this.animation.wrapMode = WrapMode.Loop;
	}

	public void OnPathComplete (Path p)
	{
		//Found a path
		if (!p.error)
		{
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
			//


		}
	}
	
	public void Update ()
	{
		// Don't have a path to move after
		if (path == null)
		{
			return;
		}		
		// Reached destination
		if (currentWaypoint >= path.vectorPath.Count)
		{

			this.animation.Stop();
			return;
		}
		
		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime/6;
		Vector3 moveTo = new Vector3(transform.position.x + dir.x, 0, transform.position.z + dir.z);

		transform.LookAt(moveTo);
		transform.position = moveTo;

		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
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
