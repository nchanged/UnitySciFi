using UnityEngine;
using System.Collections;

public class UnitBar : MonoBehaviour {
	
	private static GameObject researchButton;
	private static GameObject buildButton;
	
	public static bool inited = false;

	public Material ReseachMaterial;
	public Material BuildMaterial;




	// Movement speed in units/sec.
	private static float speed = 20.0f;
	
	// Time when the movement started.
	private static float startTime;
	private static float destinationPosition;
	private static float destinationSpeed;
	

	private static bool move = false;
	private static bool hideButtons = false;
	private static float opacity = 0.0f;
	
	public static void show()
	{
		
		if ( researchButton != null ){
			GameObject.Destroy(researchButton);
			Debug.Log("removing buttons");
		}
		if ( buildButton != null ){
			GameObject.Destroy(buildButton);
		}
		
		int bWidth = 75;
		UIObject _researchButton = new UIObject("gui/research-button", "research-button", bWidth, bWidth );
		_researchButton.setPosition(new Vector2(Screen.width/2 - bWidth/2,-95));
		researchButton = UI.attach(_researchButton);
		
		
		UIObject bButton = new UIObject("gui/build-button", "build-button", bWidth, bWidth );
		bButton.setPosition(new Vector2(Screen.width / 2 + bWidth/2,-95));
		buildButton = UI.attach(bButton);

		startTime = Time.time;
		// Calculate the journey length.
		destinationPosition = 50;
		destinationSpeed = 10;
		opacity=0;
		move = true;
		hideButtons = false;
	}
	
	public static void hide()
	{
		destinationPosition = -95;
		destinationSpeed = 100;
		move = true;
		hideButtons = true;
		opacity = 1;
	}
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	
	// Update is called once per frame
	void Update () {
		
		
		if (UnitBar.researchButton != null && UnitBar.buildButton != null && move == true) {
			

			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered/destinationSpeed;
		
			Vector3 destinationResearch = new Vector3(researchButton.transform.position.x, destinationPosition, 1);
			Vector3 destinationBuild = new Vector3(buildButton.transform.position.x, destinationPosition, 1);

			if ( hideButtons ){
				opacity = opacity - 0.2f;
				if ( opacity < 0){
					opacity = 0;
				}
			} else {
				opacity = opacity + 0.04f;
				if ( opacity > 1){
					opacity = 1;
				}
			}
			
			researchButton.renderer.materials[0].SetFloat("_Alpha",opacity);
			buildButton.renderer.materials[0].SetFloat("_Alpha",opacity);
			
			researchButton.transform.position 
				= Vector3.Lerp(researchButton.transform.position,destinationResearch, fracJourney);

			buildButton.transform.position 
				=  Vector3.Lerp(buildButton.transform.position,  destinationBuild, fracJourney);
			if ( destinationResearch == researchButton.transform.position ){
				opacity = 1;
				move = false;
			}
		}
		
	}
}

