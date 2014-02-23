using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	void Update () {
		transform.RotateAround(transform.position, Vector3.up, 20*Time.deltaTime);
	}
}
