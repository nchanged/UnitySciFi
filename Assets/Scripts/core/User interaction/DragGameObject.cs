using UnityEngine;
using System.Collections;

public class DragGameObject  {

	public static IDraggable GetDraggable(GameObject target)
	{
		// Iterate over Selectable components
		Component[] draggableComponents 
			= target.GetComponents(typeof(IDraggable));

		for (int i = 0; i<draggableComponents.Length; i++){
			// If the class inherits draggabl inteface
			if (draggableComponents[i] is IDraggable)
				return draggableComponents[i] as IDraggable;
		}
		return null;
	}
	// Return bool that tells camera to stop or procced with moving
	public static bool DispatchDrag(IDraggable draggableComponent, Vector3 pos)
	{
		Ray ray = Camera.main.ScreenPointToRay(pos);

		Plane hPlane = new Plane(Vector3.up, Vector3.zero);
		float distance = 0; 
		if (hPlane.Raycast(ray, out distance)){
			Vector3 pointerPosition = ray.GetPoint(distance);

			return draggableComponent.OnDragMove(pointerPosition);
		}
		return false;
	}

	public static void DispatchDragStop(IDraggable draggableComponent)
	{
		draggableComponent.OnDragStop();
	}
}
