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
		float posX = float.Parse(json["x"]);
		float posZ = float.Parse(json["z"]);
		Vector3 position = new Vector3(posX,0,posZ);
		GameObject instance = (GameObject)Instantiate(Resources.Load(json["name"]), position, new Quaternion());
		instance.transform.parent = DynamicObjects.transform;

		//Save instance to cache
		Storage.GameObjectCache.Add(json["_id"], instance);

		//Set Id, Name, UserId, and MapId to object instance
		Component component = instance.GetComponent(typeof(IDentifiable));
		IDentifiable identification = component as IDentifiable;
		identification.ObjectId = json["_id"];
		identification.ObjectName = json["name"];
		identification.UserId = json["owner"];
		identification.MapId = json["map"];
	}
}
