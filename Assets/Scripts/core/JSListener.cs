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
		bool isBuilding = (Int32.Parse(json["is_ready"]) == 1);

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
				instance.transform.position = new Vector3(posX, -10, posZ);
				GameObject plasmaBall = (GameObject)Instantiate(Resources.Load("plasmaball"), Vector3.zero, new Quaternion());
				plasmaBall.transform.parent = instance.transform;
				plasmaBall.transform.position = new Vector3(0f, 10.5f, 0f);
			}
		}
	}
}
