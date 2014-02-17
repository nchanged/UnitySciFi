using UnityEngine;
using System.Collections;

public class PlacementCollider : MonoBehaviour
{
	void OnTriggerEnter (Collider enteringObject)
	{
		if (enteringObject.transform.gameObject.layer == LayerMask.NameToLayer ("PlacementCollider")) {
			Transform parent = gameObject.transform.parent.transform;

			// Grab the script which implements ISelectable
			ISelectable selectableComponent = parent.gameObject.GetComponent (typeof(ISelectable)) as ISelectable;

			// Only change the indicator color of the object that is selected.
			if (selectableComponent.IsSelected) {
				Material redIndicator = Resources.Load ("SelectedIndicator_Material_Red", typeof(Material)) as Material;
				parent.FindChild ("SelectedIndicator").gameObject.renderer.material = redIndicator;

				IDraggable draggableComponent = parent.gameObject.GetComponent (typeof(IDraggable)) as IDraggable;
				draggableComponent.CurrentPositionValid = false;
			}
		}
	}
	
	void OnTriggerExit (Collider exitingObject)
	{
		if (exitingObject.transform.gameObject.layer == LayerMask.NameToLayer ("PlacementCollider")) {
			Transform parent = gameObject.transform.parent.transform;

			ISelectable selectableComponent = parent.gameObject.GetComponent (typeof(ISelectable)) as ISelectable;

			if (selectableComponent.IsSelected) {
				Material greenIndicator = Resources.Load ("SelectedIndicator_Material", typeof(Material)) as Material;
				parent.FindChild ("SelectedIndicator").gameObject.renderer.material = greenIndicator;

				IDraggable draggableComponent = parent.gameObject.GetComponent (typeof(IDraggable)) as IDraggable;
				draggableComponent.CurrentPositionValid = true;
			}
		}
	}
}
