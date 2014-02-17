using UnityEngine;
using System.Collections;

public class Cannon_RangeCollider : MonoBehaviour {

    private Cannon parent;

    void Start()
    {
        parent = transform.parent.GetComponent<Cannon>();
    }

    void OnTriggerStay(Collider enteringObject)
    {
        parent.EnemyInRange(enteringObject);
    }

    void OnTriggerExit(Collider exitingObject)
    {
        parent.EnemyLeftRange(exitingObject);
    }
}
