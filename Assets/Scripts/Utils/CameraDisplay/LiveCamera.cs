using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiveCamera : MonoBehaviour
{
    public Camera targettedCamera;

    // Update Function
    void Update()
    {
        GetComponent<RawImage>().texture = targettedCamera.activeTexture;
    }
}
