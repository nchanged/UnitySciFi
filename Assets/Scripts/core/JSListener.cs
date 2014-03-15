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
			m_View.View.BindCall("InitUnit", (System.Action<string>)this.InitUnit);
			m_View.View.BindCall("InitBuilding", (System.Action<string>)this.InitBuilding);
			m_View.View.BindCall("InitUserProfile", (System.Action<string>)this.InitUserProfile);
		}
	}

	void InitUnit(string unitString)
	{
		this.placeOnScene(unitString, Storage.UnitList);
	}

	void InitBuilding(string buildingString)
	{
		this.placeOnScene(buildingString, Storage.BuildingList);
	}

	void placeOnScene(string jsonString, List<GameObject> objectCache)
	{
		//Place an instance on the scene
		var json = JSON.Parse (jsonString);
		float posX = float.Parse(json["x"]);
		float posZ = float.Parse(json["z"]);
		Vector3 position = new Vector3(posX,0,posZ);
		GameObject instance = (GameObject)Instantiate(Resources.Load(json["name"]), position, new Quaternion());
		instance.transform.parent = DynamicObjects.transform;

		//Save instance to cache
		objectCache.Add (instance);

		//Set Id, UserId, and MapId to object instance
		Component component = instance.GetComponent(typeof(IDentifiable));
		IDentifiable asdf = component as IDentifiable;
		asdf.GameObjectId = json["_id"];
		asdf.UserId = json["owner"];
		asdf.MapId = json["map"];
	}
}
