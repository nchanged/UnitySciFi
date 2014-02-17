using UnityEngine;
using System.Collections;

public abstract class Unit : MonoBehaviour, ISelectable {

	private bool unitSelected = false;

	void Start () {
	
	}

	void Update () {
	
	}

	public void OnSelect()
	{
		SelectGameObject.HighlightObject (gameObject);
		unitSelected = true;
	}
	
	public void OnDeselect()
	{
		SelectGameObject.UnHightlightObject (gameObject);
		unitSelected = false;
	}

	public bool IsSelected
	{
		get{return unitSelected;}
		set{unitSelected = value;}
	}
}
