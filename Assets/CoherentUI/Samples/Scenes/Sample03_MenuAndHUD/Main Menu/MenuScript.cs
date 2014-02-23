using UnityEngine;
using System.Collections;
using Coherent.UI;

public class MenuScript : MonoBehaviour {
	
	
	private CoherentUIView ViewComponent;

	// Use this for initialization
	void Start ()
	{
		ViewComponent = GetComponent<CoherentUIView>();
		if (ViewComponent)
		{
			ViewComponent.OnReadyForBindings += this.RegisterBindings;
		}
	
		ViewComponent.ReceivesInput = true;
	}
	
	void Update ()
	{
		
	}	
	
	private void RegisterBindings(int frame, string url, bool isMain)
	{
		if (isMain)
		{
			var view = ViewComponent.View;
			if (view != null)
			{
				view.BindCall("NewGame", (System.Action)this.NewGame);
			}
		}
	}
	
	private void NewGame()
	{
		this.StartCoroutine(LoadGameScene());
	}
	
	IEnumerator LoadGameScene()
	{
		// Display a loading screen
		var viewComponent = GetComponent<CoherentUIView>();
		viewComponent.View.Load("coui://UIResources/MenuAndHUD/loading/loading.html");
		// The game level is very simple and loads instantly;
		// Add some artificial delay so we can display the loading screen.
		yield return new WaitForSeconds(2.5f);
		
		// Load the game level
		Application.LoadLevelAsync("game");
	}
}
