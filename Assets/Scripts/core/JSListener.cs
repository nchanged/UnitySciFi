using System;
using UnityEngine;
using System.Collections;
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
			m_View.View.BindCall("InitUserProfile", (System.Action<string>)this.InitUserProfile);
		}
	}

	void InitUnit(string unitString)
	{
		var unitJson = JSON.Parse (unitString);
		Debug.Log(unitJson);

		float unitPosX = float.Parse(unitJson["x"]);
		float unitPosZ = float.Parse(unitJson["z"]);
		Vector3 unitPosition = new Vector3(unitPosX,0,unitPosZ);

		GameObject newUnit = (GameObject)Instantiate(Resources.Load(unitJson["name"]), unitPosition, new Quaternion());
		newUnit.transform.parent = DynamicObjects.transform;
		
		Unit unitScript = (Unit)newUnit.GetComponent(typeof(Unit));
		unitScript.UnitId = unitJson["_id"];
		unitScript.OwnerId = unitJson["owner"];
		unitScript.MapId = unitJson["map"];
	}


	// Update is called once per frame
	void Update () {

	}
}
