using UnityEngine;
using System.Collections;
using System;

public abstract class Building : MonoBehaviour, ISelectable, IDraggable
{
	private bool unitSelected = false;
	private Vector3 lastValidPosition;
	private bool currentPositionValid = true;




	private bool upgradeButtonShown = false;
	private GameObject upgradeButton;
	private GameObject buildButton;




	void Start()
    {

		lastValidPosition = gameObject.transform.position;
    }

    void Update()
    {
		if ( upgradeButton) {
			//UI.animateFromBottom(upgradeButton,0);
		}
    }


	private Vector3 dragStartPosition = Vector3.zero;
	private Vector3 dragStartUnitPosition = Vector3.zero;
	// Dragging
	public bool OnDragMove(Vector3 position)
	{
		if (unitSelected) {
			if ( dragStartPosition == Vector3.zero ){
				dragStartPosition = position;
			}
			if ( dragStartUnitPosition == Vector3.zero)
			{
				dragStartUnitPosition = transform.position;
			}

			float xDiff = position.x - dragStartPosition.x;
			float zDiff = position.z - dragStartPosition.z;


			transform.position = dragStartUnitPosition + position - dragStartPosition;
			return true;
		}
		return false;
	}

	// Dragging stopped
	public void OnDragStop()
	{
		dragStartPosition = Vector3.zero;
		dragStartUnitPosition = Vector3.zero;
		if(!currentPositionValid)
		{
			gameObject.transform.position = lastValidPosition;
		}
		else
		{
			lastValidPosition = gameObject.transform.position;
			AstarPath.active.Scan();
			//AstarPath.active.UpdateGraphs (gameObject.collider.bounds);
		}
	}
	// Selecting unit
	public void OnSelect()
	{
		//showInfoButton.Show();


		UnitBar.show();
		unitSelected = true;

	}
	// Deselecting unit
	public void OnDeselect()
	{
		unitSelected = false;
		UnitBar.hide();
	}

	public bool IsSelected
	{
		get{return unitSelected;}
		set{unitSelected = value;}
	}

	public bool CurrentPositionValid 
	{
		get{return currentPositionValid;}
		set{currentPositionValid = value;}
	}
}
