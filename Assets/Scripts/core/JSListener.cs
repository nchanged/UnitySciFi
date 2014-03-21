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
			m_View.View.BindCall("InitUserProfile", (System.Action<string>)this.InitUserProfile);
			m_View.View.BindCall("InitBuildingPlaceHolder", (System.Action<string>)this.InitBuildingPlaceHolder);
		}
	}

	void InitGameObject(string jsonString)
	{
		//Place an instance on the scene
		var json = JSON.Parse (jsonString);
		string objectId = json["id"];
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
		identification.ObjectId = objectId;
		identification.ObjectName = objectName;
		identification.UserId = userId;
		identification.MapId = mapId;
		identification.IsReady = isReady;
		identification.IsBuilding = isBuilding;

		//Save instance to cache
		Storage.GameObjectCache.Add(objectId, instance);
	
		if(!isReady)
		{
			if(isBuilding)
			{
				GameObject plasmaBall = (GameObject)Instantiate(Resources.Load("plasmaball"));
				BoxCollider collider = instance.GetComponent("BoxCollider") as BoxCollider;
				Vector3 colliderSize = collider.size;

				plasmaBall.transform.parent = instance.transform;
				plasmaBall.transform.position = new Vector3(posX, 0.2f, posZ);
				plasmaBall.transform.localScale = colliderSize * 2;
			}
		}
	}

	void InitBuildingPlaceHolder(string jsonString)
	{
		var json = JSON.Parse (jsonString);
		string objectName = json["name"];
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
			}
		}
	}
}
