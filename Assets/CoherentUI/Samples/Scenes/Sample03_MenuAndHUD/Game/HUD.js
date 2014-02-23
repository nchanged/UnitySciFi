#pragma strict
#if UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
import Coherent.UI.Binding; // to use View.TriggerEvent with extra arguments

private var View : Coherent.UI.View;
private var CurrentDirection : float;

// CharacterMotor component to be disabled when a Click-To-Focus view has gained focus
public var characterMotor : CharacterMotor;


function Start () {
	var viewComponent = GetComponent(typeof(CoherentUIView)) as CoherentUIView;
	
	viewComponent.OnViewCreated += this.ViewCreated;
	viewComponent.OnViewDestroyed += this.ViewDestroyed;
	
	CurrentDirection = 0;
	
	var uiSystem = Component.FindObjectOfType(typeof(CoherentUISystem)) as CoherentUISystem;
	// get notified when a Click-to-focus view gains or loses focus
	uiSystem.OnViewFocused += this.ViewFocused;
}

function ViewCreated(view : Coherent.UI.View) {
	View = view;
	var viewComponent = GetComponent(typeof(CoherentUIView)) as CoherentUIView;
	Debug.LogWarning(String.Format("View {0} created", viewComponent.Page));
}

function ViewDestroyed() {
	View = null;
}

function ViewFocused(focused : boolean) {
	if (characterMotor) {
		// enable or disable the character movements
		characterMotor.canControl = !focused;
	}
}

function Update () {
	if (View != null)
	{
		var direction = this.transform.rotation.eulerAngles.y;
		if (Mathf.Abs(direction - CurrentDirection) > 2)
		{
			View.TriggerEvent("SetAbsoluteCompassRotation", direction);
			CurrentDirection = direction;
		}
	}
}

#endif
