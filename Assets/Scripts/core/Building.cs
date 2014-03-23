using UnityEngine;
using System.Collections;
using System;

public abstract class Building : MonoBehaviour, ISelectable, IDraggable, IDentifiable
{
	public string ObjectId {get;set;}
	public string ObjectName {get;set;}
	public string UserId {get;set;}
	public string MapId {get;set;}
	public bool IsReady {get;set;}
	public bool IsBuilding {get;set;}
	public bool IsPlaceholder {get;set;}

	private bool unitSelected = false;
	private Vector3 lastValidPosition;
	private bool currentPositionValid = true;

	public virtual void Start()
    {
		lastValidPosition = gameObject.transform.position;
		NavMeshObstacle navMeshObstacle = (NavMeshObstacle)gameObject.AddComponent("NavMeshObstacle");
		navMeshObstacle.carving = true;
    }

	void Update()
	{
	}

	private Vector3 dragStartPosition = Vector3.zero;
	private Vector3 dragStartUnitPosition = Vector3.zero;

	// Dragging
	public bool OnDragMove(Vector3 position)
	{
		if (unitSelected) {
			if ( dragStartPosition == Vector3.zero ){
				dragStartPosition = position;
			}
			if ( dragStartUnitPosition == Vector3.zero)
			{
				dragStartUnitPosition = transform.position;
			}

			transform.position = dragStartUnitPosition + position - dragStartPosition;
			return true;
		}
		return false;
	}

	// Dragging stopped
	public void OnDragStop()
	{
		dragStartPosition = Vector3.zero;
		dragStartUnitPosition = Vector3.zero;
		if(!currentPositionValid)
		{
			gameObject.transform.position = lastValidPosition;
		}
		else
		{
			lastValidPosition = gameObject.transform.position;
		}
	}

	// Selecting unit
	public void OnSelect()
	{
		unitSelected = true;
	}
	// Deselecting unit
	public void OnDeselect()
	{
		unitSelected = false;
	}

	public bool IsSelected
	{
		get{return unitSelected;}
		set{unitSelected = value;}
	}


	public bool CurrentPositionValid 
	{
		get{return currentPositionValid;}
		set{currentPositionValid = value;}
	}
}
