using UnityEngine;
using System.Collections;
using SimpleJSON;

public class JSListener : MonoBehaviour {


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
			m_View.View.BindCall("InitUnit", (System.Action<string, string, string, string>)this.InitUnit);
			m_View.View.BindCall("InitUserProfile", (System.Action<string>)this.InitUserProfile);
		}
	}

	void InitUnit(string id, string name, string owner, string xzPosition)
	{
		Debug.Log("Init: " + name + " id - " + id + " owner - " + owner + " position " + xzPosition);
	}


	// Update is called once per frame
	void Update () {

	}
}
