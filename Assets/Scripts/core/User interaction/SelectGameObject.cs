using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectGameObject {

	// Current list of selected object
	private static List<ISelectable> selected
		= new List<ISelectable>();	

	// Deselect all object
	public static void DeselectAll()
	{
		for (int a = 0; a<selected.Count; a++){
			ISelectable cSelected = selected[a];
			cSelected.OnDeselect();
			selected.Remove(cSelected);
		}
	}

	public static bool SelectionPresent()
	{
		return selected.Count > 0;
	}

	public static ISelectable GetObjectByIndex(int index)
	{
		return index < selected.Count ? selected[index] : null;
	}

	public static void Dispatch(GameObject target)
	{

		// Iterate over Selectable components
		Component[] selectableComponents 
			= target.GetComponents(typeof(ISelectable));
		
		for (int i = 0; i<selectableComponents.Length; i++){
			// If the class inherits selectable inteface
			if (selectableComponents[i] is ISelectable){
				ISelectable selectableObject = selectableComponents[i] as ISelectable;
				
				ISelectable alreadySelected = null;

				// In future, we may allow groups to be selected
				// But for now, let's just reset em all
				for (int a = 0; a<selected.Count; a++){
					ISelectable cSelected = selected[a];
					if ( alreadySelected != cSelected){
						cSelected.OnDeselect();
						selected.Remove(cSelected);
					}
				}

				// call OnSelect method
				// First check if it's there alread
				// We don't want to select it twice, do we?
				if ( selected.Contains(selectableObject) == false) {
					selectableObject.OnSelect();
					// Adding it to the list of selected object
					selected.Add(selectableObject);
					alreadySelected = selectableObject;
				} else {
					alreadySelected = selectableObject;
				}
			}
		}
	}

	public static void HighlightObject(GameObject objectToSelect)
	{
		// Assigns a material named "Assets/Resources/SelectedIndicator_Material" to the object.
		Material indicatorMaterial = Resources.Load("SelectedIndicator_Material", typeof(Material)) as Material;
		GameObject selectedIndicator = objectToSelect.transform.Find ("SelectedIndicator").gameObject;
		selectedIndicator.renderer.material = indicatorMaterial;
	}
	public static void UnHightlightObject(GameObject objectToDeSelect)
	{
		Material[] emptyMaterialsList = new Material[0];
		GameObject selectedIndicator = objectToDeSelect.transform.Find ("SelectedIndicator").gameObject;
		selectedIndicator.renderer.materials = emptyMaterialsList;
	}
}
