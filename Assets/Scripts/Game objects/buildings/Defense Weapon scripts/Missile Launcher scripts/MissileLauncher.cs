using UnityEngine;
using System;
using System.Collections;

public class MissileLauncher : Building
{

    public GameObject Ammunition;
    public float FireRate = 1f;
    public Transform MissileSpawnPoint;
    public Transform MissileLauncherHead;

    private Transform enemyTarget;
    private float nextFireTime;

    void Start() { }

    void Update()
    {
        // Enemy is in range.
        if (this.enemyTarget)
        {
            MissileLauncherHead.LookAt(enemyTarget.position);

            if (Time.time > nextFireTime)
            {
                FireMissile();
            }
        }
    }

    public void EnemyInRange(Collider enteringObject)
    {
        if (!enemyTarget)
        {
            if (enteringObject.gameObject.tag == "Air_Enemy")
            {
                nextFireTime = Time.time;
                enemyTarget = enteringObject.gameObject.transform;
            }
        }
    }

    public void EnemyLeftRange(Collider exitingObject)
    {
        // Automatically by Unity when any object leaves the current gameObject's collider.
        if (exitingObject.gameObject.transform == enemyTarget)
        {
            enemyTarget = null;
        }
    }

    void FireMissile()
    {
        nextFireTime = Time.time + FireRate;

        //Create a missile in the correct position and rotation
        GameObject missileObject = Instantiate(Ammunition, MissileSpawnPoint.position, MissileLauncherHead.rotation) as GameObject;
        //missileObject.GetComponent(Missile).Target = this.enemyTarget;


        Missile missile = (Missile)missileObject.GetComponent<Missile>();
        missile.Target = this.enemyTarget;
    }
}
