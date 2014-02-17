using UnityEngine;
using System.Collections;

public class AirEnemy : MonoBehaviour {
	
	public float Health;
	public float MinAltitude = 4.0f;
	public float MaxAltitude = 5.5f;
	public float MinSpeed = 20.0f;
	public float MaxSpeed = 25.0f;
	public float healthBarLength = 10.0f;
	
	private float maxHealth;
	private float movementSpeed;
	
	// Use this for initialization
	void Start () {
		Health = 250;
		
		maxHealth = Health;
		
		healthBarLength = Screen.width / 10;
		
		movementSpeed = Random.Range(MinSpeed, MaxSpeed);
		
		
		Vector3 temp = transform.position;
		temp.y = Random.Range(MinAltitude, MaxAltitude);
		transform.position = temp;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
	}
	
	public void TakeDamage (float damageAmount)
	{
		Health -= damageAmount;
		
		AdjustHealthBar();
		
		if(Health <= 0)
		{
			explode ();
			return;
		}
	}
	
	private void explode()
	{
		Destroy(gameObject);
	}

	public void AdjustHealthBar() {
		
		healthBarLength = (Screen.width / 10) * (Health /(float)maxHealth);
	}
}