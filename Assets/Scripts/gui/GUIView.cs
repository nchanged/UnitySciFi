using UnityEngine;
using System.Collections;


public class GUIView : MonoBehaviour {


	public void Test()
	{
		// show the options
		Debug.Log("From js aha!!");
	}

	// Use this for initialization
	void Start () {
		CoherentUIView viewComponent = GetComponent(typeof(CoherentUIView)) as CoherentUIView;
		viewComponent.OnViewCreated += this.ViewCreated;

	}
	#if UNITY_EDITOR
	public static Coherent.UI.View core;
	void ViewCreated(Coherent.UI.View core) {
		GUIView.core = core;
	
	}
	#else
	public static Coherent.UI.Mobile.View core;
	void ViewCreated(Coherent.UI.Mobile.View core) {
		GUIView.core = core;

	}

	#endif





}
