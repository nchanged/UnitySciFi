using UnityEngine;
using System.Collections;

public class ArchiveInputForward : MonoBehaviour {
	
	// Use this for initialization
	void Start ()
	{
		CoherentUIView view = GetComponent<CoherentUIView>();
		
		if (view == null)
		{
			Debug.LogError("The GameObject must have a CoherentUIView component!");
			return;
		}
		// This is the only view in the sample and we'll always forward the input to it
		view.ReceivesInput = true;
	}
	
	void Update ()
	{
		
	}
}
