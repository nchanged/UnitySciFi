using UnityEngine;
using System.Collections;

public class MissileLauncher_RangeCollider : MonoBehaviour {

	private MissileLauncher parent;

	void Start(){
		parent = transform.parent.GetComponent<MissileLauncher>();
	}

	void OnTriggerStay(Collider enteringObject){
		parent.EnemyInRange(enteringObject);
	}

	void OnTriggerExit(Collider exitingObject){
		parent.EnemyLeftRange(exitingObject);
	}
}
