using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UI : MonoBehaviour {
	
	//private static List<GameObject> createdGameObjects
	//	= new List<GameObject>();
	
	private static  Dictionary<string,UIObject> instances 
		= new Dictionary<string, UIObject>();
	
	public Camera camera;
	public static GameObject group;
	public GameObject GuiGroup;
	
	public static GameObject attach(UIObject o)
	{
		
		Vector2 userPosition = o.getPosition();
		
		
		Vector3 correctedPosition 
			= new Vector3( userPosition.x, userPosition.y, 1);
		
		GameObject inst 
			= Instantiate(Resources.Load(o.getResourceName()), correctedPosition, Quaternion.identity) as GameObject;
		
		
		inst.transform.parent = group.transform;
		inst.transform.localScale = new Vector3(o.getWidth(), o.getHeight(), 1);
		inst.name = o.getName();
		return inst;
	}
	
	// Use this for initialization
	void Start () {
		if (  group == null ){
			group = GuiGroup;
		}
	}
	
	// Update is called once per frame
	void Update () {
		this.camera.orthographicSize  = Screen.height / 2;
		this.camera.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
	}
}

