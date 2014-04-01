using System;
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using Coherent.UI.Binding;
#else
using Coherent.UI.Mobile.Binding;
#endif

public class JSTrigger : MonoBehaviour {

	public static void StartBuildingConstruction(GameObject parentGameObject)
	{
		string placeholderId = Guid.NewGuid().ToString();
		Building buildingComponent = parentGameObject.GetComponent<Building>() as Building;
		buildingComponent.PlaceholderId = placeholderId;

		string unitId = buildingComponent.ObjectId;
		float xPosition = parentGameObject.transform.position.x;
		float zPosition = parentGameObject.transform.position.z;

		Storage.GameObjectCache.Add(placeholderId, parentGameObject);
		GUIView.core.TriggerEvent("startBuilding", placeholderId, unitId, xPosition, zPosition);
	}

	public static void CancelBuildingConstructin(string buildingId)
	{
		GUIView.core.TriggerEvent ("cancelBuilding", buildingId);
	}

	public static void ConfirmBuildingConstructionComplete(string buildingId)
	{
		GUIView.core.TriggerEvent ("isBuildingComplete", buildingId);
	}
}
