﻿using UnityEngine;
using System.Collections;
using System;

public abstract class Building : MonoBehaviour, ISelectable, IDraggable, IDentifiable
{
	public string InstanceId {get;set;}
	public string ObjectId {get;set;}
	public string ObjectName {get;set;}
	public string UserId {get;set;}
	public string MapId {get;set;}
	public bool IsReady {get;set;}
	public bool IsBuilding {get;set;}
	public bool IsPlaceholder {get;set;}
	public string PlaceholderId {get;set;}
	public long ReadyEstimation {get;set;}

	private bool unitSelected = false;
	private Vector3 lastValidPosition;
	private bool currentPositionValid = true;

	public virtual void Start()
    {
		lastValidPosition = gameObject.transform.position;
		NavMeshObstacle navMeshObstacle = (NavMeshObstacle)gameObject.AddComponent("NavMeshObstacle");
		navMeshObstacle.carving = true;
    }

	float timer = 0f;
	void Update()
	{
	
		if ( IsReady == false ) {
			if ( ReadyEstimation > 0 ){
				timer += Time.deltaTime * 1000;


				//Debug.Log(ReadyEstimation);
				UpdateProgressBar();
			}
		}
	}


	void UpdateProgressBar()
	{
		//Debug.Log(ReadyEstimation);
		TimeSpan ts = TimeSpan.FromMilliseconds(ReadyEstimation - (long) timer);
		string readableFormat = string.Format("{0:D2} hours {1:D2} min {2:D2} sec", 
		                              //ts.Days, 
		                              ts.Hours, 
		                              ts.Minutes, 
		                              ts.Seconds);
		foreach (Transform child in transform)
		{
			if ( child.gameObject.name == "progressbar" ) {
				Transform pbackground = child.transform.Find("background");
				if (pbackground != null) {
					Transform eta = pbackground.Find("ETA");
					if (eta != null ){
						eta.gameObject.GetComponent<TextMesh>().text = readableFormat;

					}

				}

				//foreach (Transform pchild in child.gameObject.transform)
				//{
				//	Debug.Log(pchild);
				//}

			}
		}
	
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
