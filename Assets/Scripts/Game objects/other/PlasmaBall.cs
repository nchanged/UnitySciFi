using UnityEngine;
using System.Collections;

public class PlasmaBall : MonoBehaviour {
	void Update ()
	{
		transform.Rotate(0,0,100*Time.deltaTime);
	}
}
