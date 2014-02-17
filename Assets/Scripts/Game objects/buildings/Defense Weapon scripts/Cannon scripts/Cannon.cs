using UnityEngine;
using System.Collections;

public class Cannon : Building
{
    public GameObject Ammunition;
    public GameObject MuzzleFlash;
    public float FireRate = 1f;
    public Transform[] BarrelArray;
    public Transform CannonBody;

    private Transform enemyTarget;
    private float nextFireTime;

    void Update()
    {
        // Enemy is in range.
        if (enemyTarget)
        {
            // Point the cannon at the enemy.
            CannonBody.transform.LookAt(enemyTarget.position);

            if (Time.time > nextFireTime)
            {
                FireCannon();
            }
        }
    }

    public void EnemyInRange(Collider enteringObject)
    {
        if (!enemyTarget && enteringObject.gameObject.tag == "Enemy")
        {
            nextFireTime = Time.time;
            enemyTarget = enteringObject.gameObject.transform;
        }
    }

    public void EnemyLeftRange(Collider exitingObject)
    {
        if (exitingObject.gameObject.transform == enemyTarget)
        {
            enemyTarget = null;
        }
    }

    void FireCannon()
    {
        //audio.Play();
        nextFireTime = Time.time + FireRate;

        foreach (Transform barrel in BarrelArray)
        {
            //Create a cannonball and muzzleflash at the same position and rotation as the barrel
            Instantiate(Ammunition, barrel.position, CannonBody.rotation);
            Instantiate(MuzzleFlash, barrel.position, CannonBody.rotation);
        }
    }
}