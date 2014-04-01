using UnityEngine;
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
	public long TakesTime {get;set;}

	private bool unitSelected = false;
	//private Vector3 lastValidPosition;
	private bool currentPositionValid = true;

	public virtual void Start()
    {
		//lastValidPosition = gameObject.transform.position;
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

	void OnTriggerStay(Collider other){
		if(other.transform.gameObject.name != "Terrain" && this.IsPlaceholder && this.currentPositionValid)
		{
			TextureSwitcher.AddSecondaryMaterial(gameObject, "DynamicMaterials/PlaceholderRed");
			this.currentPositionValid = false;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if(other.transform.gameObject.name != "Terrain" && this.IsPlaceholder)
		{
			TextureSwitcher.AddSecondaryMaterial(gameObject, "DynamicMaterials/PlaceholderGreen");
			this.currentPositionValid = true;
		}
	}

	void UpdateProgressBar()
	{
		//Debug.Log(ReadyEstimation);

		long timeLeft = ReadyEstimation - (long) timer;
		float percents = 1 - ((float)timeLeft / (float)TakesTime);

		float materialShiftPercentage = (50f / 100f * percents);


		TimeSpan ts = TimeSpan.FromMilliseconds(timeLeft);
		string readableFormat = string.Format("{0:D2} h {1:D2} min {2:D2} sec", 
		                              //ts.Days, 
		                              ts.Hours, 
		                              ts.Minutes, 
		                              ts.Seconds);
		foreach (Transform child in transform)
		{
			if ( child.gameObject.name == "progressbar" ) {
				Transform eta = child.transform.Find("ETA");
				Transform title = child.transform.Find("Title");

				Transform container = child.transform.Find("container");
				container.gameObject.renderer.material.SetTextureOffset ("_MainTex", new Vector2(materialShiftPercentage,0));
				if ( title != null ) {
					title.gameObject.GetComponent<TextMesh>().text = "Building " + ObjectName;
				}

				if (eta != null ){
					eta.gameObject.GetComponent<TextMesh>().text = readableFormat;
				}


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
		/*if(!currentPositionValid)
		{
			gameObject.transform.position = lastValidPosition;
		}
		else
		{
			lastValidPosition = gameObject.transform.position;
		}*/
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

	public void OnConstructionStart()
	{

	}
	public void OnConstructionCancel()
	{
		JSTrigger.CancelBuildingConstructin (this.InstanceId);
		Destroy (gameObject);
	}
	public void OnConstructionComplete(bool confirmedByServer)
	{
		if(confirmedByServer)
		{
			this.IsReady = true;
			Destroy(gameObject.GetComponent("PulseGlow"));
			TextureSwitcher.RevertToDefault(gameObject);
			Destroy(transform.Find("ProgressBar").gameObject);
		}
		else
		{
			JSTrigger.ConfirmBuildingConstructionComplete(this.InstanceId);
		}
	}
}
