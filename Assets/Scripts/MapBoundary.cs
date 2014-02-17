using UnityEngine;
using System.Collections;

public class MapBoundary : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit(Collider collisionObject){
		if(collisionObject.gameObject.tag == "Air_Enemy" || collisionObject.gameObject.tag == "Enemy")
		{
			print (collisionObject.gameObject.name);
			Destroy(collisionObject.gameObject);
		}
	}
}
