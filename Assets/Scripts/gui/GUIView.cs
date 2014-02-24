using UnityEngine;
using System.Collections;

public class GUIView : MonoBehaviour {
	public static Coherent.UI.View View;
	// Use this for initialization
	void Start () {
		CoherentUIView viewComponent = GetComponent(typeof(CoherentUIView)) as CoherentUIView;
		viewComponent.OnViewCreated += this.ViewCreated;
	}

	void ViewCreated(Coherent.UI.View view) {
		//View = view;
		View = view;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
