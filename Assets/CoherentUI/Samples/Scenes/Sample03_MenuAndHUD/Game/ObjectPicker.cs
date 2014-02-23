#if UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
#define COHERENT_UNITY_STANDALONE
#endif
using UnityEngine;
using System.Collections;

#if COHERENT_UNITY_STANDALONE || UNITY_EDITOR
public class ObjectPicker : MonoBehaviour {
	
	private Camera m_MainCamera;
	private CoherentUISystem m_UISystem;

	// Use this for initialization
	void Start () {
		m_MainCamera = GameObject.Find("Main Camera").camera;
		m_UISystem = Component.FindObjectOfType(typeof(CoherentUISystem)) as CoherentUISystem;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_UISystem.HasFocusedView)
		{
			return;
		}
		// Reset input processing for all views
		foreach (var item in m_UISystem.UIViews) {
			if (!item.ClickToFocus) {
				item.ReceivesInput = false;
			}
		}
		
		var cameraView = m_MainCamera.gameObject.GetComponent<CoherentUIView>();
		if (cameraView && !cameraView.ClickToFocus)
		{
			var view = cameraView.View;
			if (view != null)
			{
				var factorY = cameraView.Height / m_MainCamera.pixelHeight;

				//Normalize the view coordinates. We need this when we use view dimensions
				//are different than the camera ones
				var normX = Input.mousePosition.x / cameraView.Width;
				var normY = 1 - Input.mousePosition.y / cameraView.Height;

				//After the normalizations, the Normalized Y coordinate will be displaced because Y coords
				//grow downwards and we have to put it back into the [0-1] range.

				//E.g. if the view height is two times smaller
				//than the camera height, the normalized Y coords will start from -1 and end to +1.
				//If the view's height is 4 times smaller, the normalized coords will start from -3 and end to +1

				//The formula puts the Y coord back in [0-1] range.
				normY = ( (normY * factorY) + (1 - factorY) ) / factorY;

				if (normX >= 0 && normX <= 1 && normY >= 0 && normY <= 1)
				{
					view.IssueMouseOnUIQuery(normX, normY);
					view.FetchMouseOnUIQuery();
					if (view.IsMouseOnView())
					{
						cameraView.ReceivesInput = true;
						return;
					}
				}
			}
		}

		
		// Activate input processing for the view below the mouse cursor
		RaycastHit hitInfo;
		if (Physics.Raycast(m_MainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo))
		{
			//Debug.Log (hitInfo.collider.name);
			
			CoherentUIView viewComponent = hitInfo.collider.gameObject.GetComponent(typeof(CoherentUIView)) as CoherentUIView;
			if (viewComponent == null)
			{
				viewComponent = hitInfo.collider.gameObject.GetComponentInChildren(typeof(CoherentUIView)) as CoherentUIView;
			}
			
			if (viewComponent != null && !viewComponent.ClickToFocus)
			{
				viewComponent.ReceivesInput = true;
				viewComponent.SetMousePosition(
					(int)(hitInfo.textureCoord.x * viewComponent.Width),
					(int)(hitInfo.textureCoord.y * viewComponent.Height));
			}
		}
	}
}
#endif
