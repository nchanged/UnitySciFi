using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UserInput : MonoBehaviour
{
	private bool lockCameraMovement = false;
	private Vector3 hitPosition = Vector3.zero;
	private Vector3 pointerPosition = Vector3.zero;
	private Vector3 cameraStartPosition = Vector3.zero;
	private Vector3 cameraMovePosition = Vector3.zero;

	public float defaultCameraY = 100;
	public float cameraMinX = 1000f;
	public float cameraMaxX = 1400f;
	public float cameraMinY = 30f;
	public float cameraMaxY = 100f;
	public float cameraMinZ = 700f;
	public float cameraMaxZ = 1300f;

	private Vector3 lastCameraPosition = Vector3.zero; // Used to calculate camera velocity
	private Vector3 cameraVelocity = Vector3.zero; // Used to smoothly decelerate camera
	private bool smoothToStop = false; // Flag that tells whether the camera should decelerate or not
	public float momentum = 9.0f; // Determines how long it takes for the camera to stop

	private float deadZoneThreshold = 15f;
	private bool withinDeadZone = true;
	private Vector3 deadZoneLeavePosition = Vector3.zero;

	private Vector2 currTouch1 = Vector2.zero;
	private Vector2 currTouch2 = Vector2.zero;
	private Vector2 lastTouch1 = Vector2.zero;
	private Vector2 lastTouch2 = Vector2.zero;
	private float currDist = 0.0f;
	private float lastDist = 0.0f;
	private float zoomSpeed = 7.0f;

	void Awake ()
	{
		Application.targetFrameRate = 60;
	}

	void Start ()
	{
		Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, defaultCameraY, Camera.main.transform.position.z);

	}

	void Update ()
	{
		bool userFingerUp = false;
		bool userFingerDown = false;
		bool userFingerPressed = false;
		pointerPosition = Vector3.zero;

		bool secondFingerUp = false;
		bool secondFingerDown = false;
		bool secondFingerPressed = false;
		Vector3 secondPointerPosition = Vector3.zero;


		if (Input.touchCount == 0)
		{
			userFingerUp = Input.GetMouseButtonUp (0);
			userFingerDown = Input.GetMouseButtonDown (0);
			userFingerPressed = Input.GetMouseButton (0);
			pointerPosition = Input.mousePosition;

			selectAndDrag (userFingerUp, userFingerDown, userFingerPressed);
			
			if (!lockCameraMovement)
			{
				moveCamera (userFingerUp, userFingerDown, userFingerPressed);
			}
		}
		else
		{
			userFingerUp = Input.GetTouch (0).phase == TouchPhase.Ended;
			userFingerDown = Input.GetTouch (0).phase == TouchPhase.Began;
			userFingerPressed = Input.GetTouch (0).phase == TouchPhase.Moved;
			pointerPosition = Input.GetTouch (0).position;

			if (Input.touchCount == 1)
			{
				selectAndDrag (userFingerUp, userFingerDown, userFingerPressed);				
				if (!lockCameraMovement)
				{
					moveCamera (userFingerUp, userFingerDown, userFingerPressed);
				}
			}
			else if (Input.touchCount == 2)
			{
				secondFingerUp = Input.GetTouch (1).phase == TouchPhase.Ended;
				secondFingerDown = Input.GetTouch (1).phase == TouchPhase.Began;
				secondFingerPressed = Input.GetTouch (1).phase == TouchPhase.Moved;
				secondPointerPosition = Input.GetTouch (1).position;
			}

			zoom(userFingerUp, userFingerPressed, secondFingerUp, secondFingerPressed, secondPointerPosition);
		}
	}

	void moveCamera (bool userFingerUp, bool userFingerDown, bool userFingerPressed)
	{
		if (userFingerDown)
		{
			hitPosition = pointerPosition;
			cameraStartPosition = Camera.main.transform.position;
			lastCameraPosition = Camera.main.transform.position;
			cameraVelocity = Vector3.zero;
			smoothToStop = false;
		}

		//if (userFingerPressed && !withinDeadZone)
		if (userFingerPressed)
		{
			cameraVelocity = Vector3.zero;
			smoothToStop = false;

			// Our camera is rotated 90degrees on the X axis.. so Z axis and Y axis are inverted.
			//pointerPosition.z = hitPosition.z = deadZoneLeavePosition.z = cameraStartPosition.y;
			pointerPosition.z = hitPosition.z = cameraStartPosition.y;

			// Add the offset of the deadZone, so that the camera doesn't suddenly jump 30f when it starts moving.
			//Vector3 deadZoneOffset = deadZoneLeavePosition - hitPosition;
			//hitPosition += deadZoneOffset;

			// Calculating camera shift
			Vector3 direction = Camera.main.ScreenToWorldPoint (pointerPosition) - Camera.main.ScreenToWorldPoint (hitPosition);
			direction *= -1;

			// Clamp X and Z for map boundaries.
			Vector3 calculatedPosition = cameraStartPosition + direction;
			cameraMovePosition = new Vector3 (
				Mathf.Clamp (calculatedPosition.x, cameraMinX, cameraMaxX),
				defaultCameraY,
				Mathf.Clamp (calculatedPosition.z, cameraMinZ, cameraMaxZ)
			);

			Camera.main.transform.position = cameraMovePosition;

			cameraVelocity = (Camera.main.transform.position - lastCameraPosition);
			lastCameraPosition = cameraMovePosition;
		}

		// Finger lifted after dragging camera.. check if we need to decelerate.
		if (userFingerUp && cameraVelocity != Vector3.zero)
		{
			smoothToStop = true;
		}

		if(smoothToStop)
		{
			Vector3 newVelocity = cameraVelocity - (cameraVelocity / momentum);
			float newXPosition = Mathf.Clamp (Camera.main.transform.position.x + newVelocity.x, cameraMinX, cameraMaxX);
			float newZPosition = Mathf.Clamp (Camera.main.transform.position.z + newVelocity.z, cameraMinZ, cameraMaxZ);
			
			Camera.main.transform.position = new Vector3(newXPosition, defaultCameraY, newZPosition);
			
			cameraVelocity = newVelocity;
			lastCameraPosition = Camera.main.transform.position; // Keep tabs on where the camera is, even when we're not moving it with a finger.

			// Don't do those ridiculously small movements such as 0.0001f
			if(Mathf.Abs (cameraVelocity.x) < 0.01 && Mathf.Abs (cameraVelocity.z) < 0.01)
			{
				smoothToStop = false;
			}
		}
	}

	/*----- Moving a draggable object below ------*/
	private IDraggable draggableComponent = null;
	private Vector3 latestDragCameraPosition;
	private Vector3 latestFingerDownPosition;
	private bool draggingOccured = false;
	private ICommandable commandableComponent = null;
	private Vector3 terrainPointed;

	void selectAndDrag (bool userFingerUp, bool userFingerDown, bool userFingerPressed)
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (pointerPosition);
		int layerMask =~(1 << 10);
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask))
		{
			if (userFingerDown)
			{
				latestFingerDownPosition = pointerPosition;
				withinDeadZone = true;

				if (hit.transform.gameObject.name == "Terrain")
				{
					terrainPointed = pointerPosition;
				}
				
				
				draggableComponent = DragGameObject.GetDraggable (hit.transform.gameObject);
				if (draggableComponent != null && SelectGameObject.GetObjectByIndex (0) == draggableComponent)
				{
					lockCameraMovement = true;
				}
				else
				{
					lockCameraMovement = false;
				}
			}

			// Once we leave the deadzone, we don't check this anymore until the next touch/mouse down.
			if(userFingerPressed && withinDeadZone)
			{
				float draggedDistance = Vector3.Distance(latestFingerDownPosition, pointerPosition);
				if(draggedDistance > deadZoneThreshold)
				{
					withinDeadZone = false;
					deadZoneLeavePosition = pointerPosition;
				}
			}

			if (userFingerUp)
			{
				if (hit.transform.gameObject.name == "Terrain")
				{
					if (withinDeadZone)
					{
						if(commandableComponent != null)
						{
							CommandGameObject.DispatchMoveCommand(commandableComponent, hit.point);
						}

						//Deselect all objects, whether we are commanding or not.
						SelectGameObject.DeselectAll ();
					}
				}
				// If we have not move a screen
				if (withinDeadZone)
				{
					SelectGameObject.Dispatch (hit.transform.gameObject);
					commandableComponent = CommandGameObject.GetCommandable(hit.transform.gameObject);
				}

				if (draggableComponent != null && draggingOccured)
				{
					DragGameObject.DispatchDragStop (draggableComponent);
					draggingOccured = false;
				}
				draggableComponent = null;
				withinDeadZone = true;
			}
		}

		if (draggableComponent != null)
		{
			if (!withinDeadZone)
			{
				lockCameraMovement = DragGameObject.DispatchDrag (draggableComponent, pointerPosition);
				draggingOccured = lockCameraMovement;
			}
		}
		else
		{
			lockCameraMovement = false;
		}
		latestDragCameraPosition = pointerPosition;
	}


	/*--- Pinch to zoom ---*/
	void zoom(bool userFingerUp, bool userFingerPressed,
	          bool secondFingerUp, bool secondFingerPressed, Vector3 secondPointerPosition)
	{
		if (Input.touchCount == 2)
		{
			currTouch1 = Input.GetTouch(0).position;
			currTouch2 = Input.GetTouch(1).position;
			currDist = Vector2.Distance(currTouch1, currTouch2);

			if(lastDist != 0.0f){
				float zoomFactor = (lastDist-currDist);
				float rotationFactor = Mathf.Tan(Camera.main.transform.eulerAngles.x * Mathf.Deg2Rad);

				Vector3 zoom = Vector3.back * zoomFactor * zoomSpeed * Time.deltaTime * -1;					
				float Y = Mathf.Clamp(Camera.main.transform.position.y + zoom.z, cameraMinY, cameraMaxY);
				float X = Mathf.Clamp(Camera.main.transform.position.x + (zoom.z * rotationFactor), cameraMinX, cameraMaxX);
				float Z = Camera.main.transform.position.z;
				Vector3 newPos = new Vector3(X,Y,Z);

				if(Y > cameraMinY && zoomFactor < 0 || Y < cameraMaxY && zoomFactor > 0){
					Camera.main.transform.position = newPos;
				}
			}

			lastDist = currDist;
			defaultCameraY = Camera.mainCamera.transform.position.y;

			if(userFingerUp || secondFingerUp)
			{
				Vector3 remainingFingerPos = userFingerUp ? secondPointerPosition : pointerPosition;
				lastDist = 0.0f;
				pointerPosition = remainingFingerPos;
				hitPosition = remainingFingerPos;
				deadZoneLeavePosition = remainingFingerPos;
				cameraStartPosition = Camera.main.transform.position;
				lastCameraPosition = Camera.main.transform.position;
				cameraVelocity = Vector3.zero;
				smoothToStop = false;
			}
		}
	}
}
