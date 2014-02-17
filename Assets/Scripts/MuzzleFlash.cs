using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour
{
    void Start()
    {
        float delayTime = gameObject.particleSystem.duration + gameObject.particleSystem.startLifetime;
        Destroy(gameObject, delayTime);
    }
}
