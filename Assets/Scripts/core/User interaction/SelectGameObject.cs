using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using Coherent.UI.Binding;
#else
using Coherent.UI.Mobile.Binding;
#endif


public class SelectGameObject : MonoBehaviour {

	// Current list of selected object
	private static List<ISelectable> selected
		= new List<ISelectable>();	


	private static List<GameObject> selectionHelperMap
		= new List<GameObject>();	

	void Start()
	{

	}
	void Update()
	{

	}
	// Deselect all object
	public static void DeselectAll()
	{
		for (int a = 0; a<selected.Count; a++){
			ISelectable cSelected = selected[a];
			cSelected.OnDeselect();
			selected.Remove(cSelected);
		}
		removeSelection();
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

				removeSelection();
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
					drawSelection(target);
					// Adding it to the list of selected object
					selected.Add(selectableObject);
					alreadySelected = selectableObject;
				} else {
					alreadySelected = selectableObject;
				}
			}
		}
	}

	public static void removeSelection()
	{
		for (int a = 0; a<selectionHelperMap.Count; a++){
			GameObject.Destroy(selectionHelperMap[a]);
		}
		selectionHelperMap = new List<GameObject>();
		GUIView.core.TriggerEvent("unitDeselected");
		
	}


	// Get GUI name
	public static string getUnitGUIName(GameObject target )
	{
		Component[] guiUnites 
			= target.GetComponents(typeof(GUIUnit));
		for (int i = 0; i<guiUnites.Length; i++){
			if (guiUnites[i] is GUIUnit){
				GUIUnit unit = guiUnites[i] as GUIUnit;
				return unit.getName();
			}
		}
		return null;
	}
	public static void drawSelection(GameObject target)
	{

		//gui.triggerEvent("showUnitBar");
		string guiName = getUnitGUIName(target);

		GUIView.core.TriggerEvent("unitSelected", guiName);
		//Debug.Log("")
		//GUIView.core.TriggerEvent("test");



		BoxCollider collider = target.GetComponent("BoxCollider") as BoxCollider;
		Vector3 colliderSize = collider.size;
		Vector3 colliderCenter = collider.center;

		//Vector3 size = new Vector3(colliderSize.x /2, 
		 //                          colliderSize.y /2 , 
		 //                          colliderSize.z /2);

		Vector3 size = new Vector3(colliderSize.x/2 - colliderCenter.x,0,colliderSize.z /2 - colliderCenter.z);
		float yPosition = target.transform.position.y*-1 + 1;
		float yRotation = target.transform.eulerAngles.y;


		Vector3 cornerScale = new Vector3(2,2,1);
		//Debug.Log(" NATURAL " + size);
		//Debug.Log("REAL " + collider.size);
		//size = collider.size;
		GameObject instRB = Instantiate(Resources.Load("gui/SelectionPart"), target.transform.localPosition, Quaternion.Euler(90, 270+yRotation, 0)) as GameObject;
		instRB.transform.parent = target.transform;
		instRB.transform.localScale = cornerScale;
		instRB.transform.localPosition = new Vector3(size.x,yPosition, size.z); 

		selectionHelperMap.Add (instRB);
		GameObject instLT = Instantiate(Resources.Load("gui/SelectionPart"), target.transform.localPosition, Quaternion.Euler(90, 90+yRotation, 0)) as GameObject;
		instLT.transform.parent = target.transform;
		instLT.transform.localScale = cornerScale;
		instLT.transform.localPosition = new Vector3(size.x*-1,yPosition, size.z*-1); 
		selectionHelperMap.Add (instLT);

		GameObject instTR = Instantiate(Resources.Load("gui/SelectionPart"), target.transform.localPosition, Quaternion.Euler(90, 180+yRotation, 0)) as GameObject;
		instTR.transform.parent = target.transform;
		instTR.transform.localScale = cornerScale;
		instTR.transform.localPosition = new Vector3(size.x*-1,yPosition, size.z); 
		selectionHelperMap.Add (instTR);


		GameObject instLB = Instantiate(Resources.Load("gui/SelectionPart"), target.transform.localPosition, Quaternion.Euler(90, 0+yRotation, 0)) as GameObject;
		instLB.transform.parent = target.transform;
		instLB.transform.localScale = cornerScale;
		instLB.transform.localPosition = new Vector3(size.x, yPosition,size.z*-1);

		selectionHelperMap.Add (instLB);

	}

}
