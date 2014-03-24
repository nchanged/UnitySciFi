using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class JSListener : MonoBehaviour {


	public GameObject DynamicObjects;
	private CoherentUIView m_View;
	
	void Start()
	{
		m_View = GetComponent<CoherentUIView>();
		if (m_View)
		{
			m_View.Listener.ReadyForBindings += HandleReadyForBindings;
		}
	}

	void InitUserProfile(string userString)
	{
		var userJson = JSON.Parse(userString);
		UserProfile.UserId = userJson["_id"];
		UserProfile.Name = userJson["name"];
		UserProfile.Session = userJson["session"];
		UserProfile.Map = userJson["map"];
	}
	
	void HandleReadyForBindings (int frameId, string path, bool isMainFrame)
	{
		if (isMainFrame)
		{
			m_View.View.BindCall("InitGameObject", (System.Action<string>)this.InitGameObject);
			m_View.View.BindCall("BuildingOrdered", (System.Action<string>)this.InitGameObject);
			m_View.View.BindCall("InitUserProfile", (System.Action<string>)this.InitUserProfile);
			m_View.View.BindCall("InitBuildingPlaceHolder", (System.Action<string>)this.InitBuildingPlaceHolder);
		}
	}

	void InitGameObject(string jsonString)
	{
		//Place an instance on the scene
		var json = JSON.Parse (jsonString);
		string instanceId = json["id"];
		string objectId = json["unit_id"];
		string objectName = json["name"];
		string userId = json["user_id"];
		string mapId = json["map_id"];
		float posX = float.Parse(json["x"]);
		float posZ = float.Parse(json["z"]);
		Vector3 position = new Vector3(posX,0,posZ);
		bool isReady = (Int32.Parse(json["is_ready"]) == 1);
		bool isBuilding = (Int32.Parse(json["is_building"]) == 1);

		GameObject instance = (GameObject)Instantiate(Resources.Load(objectName), position, new Quaternion());
		instance.transform.parent = DynamicObjects.transform;
		
		//Set IDentifiable fields
		Component component = instance.GetComponent(typeof(IDentifiable));
		IDentifiable identification = component as IDentifiable;
		identification.InstanceId = instanceId;
		identification.ObjectId = objectId;
		identification.ObjectName = objectName;
		identification.UserId = userId;
		identification.MapId = mapId;
		identification.IsReady = isReady;
		identification.IsBuilding = isBuilding;

		//Save instance to cache
		Storage.GameObjectCache.Add(instanceId, instance);
	
		if(!isReady)
		{
			if(isBuilding)
			{
				GameObject spinner = (GameObject)Instantiate(Resources.Load("ConstructionSpinner"));
				BoxCollider collider = instance.GetComponent("BoxCollider") as BoxCollider;
				float spinnerScale = (collider.size.x > collider.size.z) ? collider.size.x * 1.5f : collider.size.z * 1.5f;
				Vector3 spinnerSize = new Vector3(spinnerScale, spinnerScale, spinnerScale);

				spinner.transform.parent = instance.transform;
				spinner.transform.position = new Vector3(posX, 0.2f, posZ);
				spinner.transform.localScale = spinnerSize;

				instance.AddComponent("PulseGlow");

				float barPosX = instance.renderer.bounds.max.x + 2f;
				float barPosY = 1f;
				float barPosZ = instance.renderer.bounds.center.z;
				Vector3 barPosition = new Vector3(barPosX, barPosY, barPosZ);
				GameObject progressBar = (GameObject)Instantiate(Resources.Load("gui/progressBar"), barPosition, new Quaternion());
				progressBar.transform.parent = instance.transform;
			}
		}
	}

	void InitBuildingPlaceHolder(string jsonString)
	{
		var json = JSON.Parse (jsonString);
		string objectName = json["name"];
		string objectId = json["unit_id"];
		bool isBuilding = (Int32.Parse(json["is_building"]) == 1);

		if(isBuilding){
			// Find the center of the screen and put the placeholder there.
			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
			RaycastHit hit;			
			if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == "Terrain"){
				Vector3 position = hit.point;
				GameObject instance = (GameObject)Instantiate(Resources.Load(objectName), position, new Quaternion());
				SelectGameObject.DeselectAll();
				SelectGameObject.Dispatch(instance);
				IDraggable draggableComponent = DragGameObject.GetDraggable (instance);
				draggableComponent.IsPlaceholder = true;
				Building buildingComponent = instance.GetComponent<Building>() as Building;
				buildingComponent.ObjectId = objectId;


				float posX = instance.renderer.bounds.max.x + 2f;
				float posY = 2.5f;
				float posZ = instance.renderer.bounds.center.z;
				Vector3 buttonsPosition = new Vector3(posX, posY, posZ);
				GameObject constructionButtons = (GameObject)Instantiate(Resources.Load("gui/ConstructionButtons"), buttonsPosition, new Quaternion());
				constructionButtons.transform.parent = instance.transform;
			}
		}
	}
}
