using UnityEngine;
using System.Collections;

public class SampleCubeRotator : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(1, 1, 1), 32.0f * Time.deltaTime);
	}
}
