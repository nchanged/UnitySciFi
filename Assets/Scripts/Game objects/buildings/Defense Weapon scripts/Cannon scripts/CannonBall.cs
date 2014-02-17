using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour
{
    public float Speed = 500;
    public float Range = 500;

    private float currentDistance;

    void Update()
    {
        // Move the cannonball "Forward" along the Z-axis
        transform.Translate(Vector3.forward * Time.deltaTime * this.Speed);

        // Calculate how much distance the cannonball has moved
        currentDistance += Time.deltaTime * this.Speed;

        // Destroy the cannonball if it has reached its max range
        if (currentDistance >= this.Range)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider enteringObject)
    {
        if (enteringObject.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}