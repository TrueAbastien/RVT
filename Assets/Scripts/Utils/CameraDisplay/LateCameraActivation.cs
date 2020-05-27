using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LateCameraActivation : MonoBehaviour
{
    [Range(0,5)] public float timeBeforeActivation = .5f;

    // On Awake Function
    void Awake()
    {
        GetComponent<Camera>().enabled = false;
        Destroy(this, timeBeforeActivation);
    }

    // On Destory Function
    private void OnDestroy()
    {
        GetComponent<Camera>().enabled = true;
    }
}
