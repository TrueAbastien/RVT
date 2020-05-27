using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inventory Item, controls the camera horizontal offset.
/// </summary>
public class VRCameraController : InventoryItem
{
    /// <summary>
    /// Camera sensitivity (vertical is useless).
    /// </summary>
    private Vector2 Sensitivity = new Vector2(.8f, .5f);

    /// <summary>
    /// Current user camera.
    /// </summary>
    private Camera UserCam;
    /// <summary>
    /// Transform of the user camera.
    /// </summary>
    private Transform camTransform;


    /// <summary>
    /// Start Function, initialize propreties.
    /// </summary>
    void Start()
    {
        UserCam = Camera.main;
        camTransform = UserCam.transform.parent;
    }

    /// <summary>
    /// Update Function, update camera rotation.
    /// </summary>
    void Update()
    {
        // Camera Movements
        camTransform.RotateAround(UserCam.transform.position, Vector3.up,
            Input.GetAxis(currentInput.ThumbX) * Sensitivity.x * Time.deltaTime * 90f);
    }

    /// <summary>
    /// On selection, rotate the current hand the other way around (if Left).
    /// </summary>
    /// <param name="hand">Current item hand</param>
    protected override void ConstructOver(VRHand.Hand hand)
    {
        if (hand == VRHand.Hand.LEFT)
        {
            Vector3 oScale = GetComponentInChildren<MeshRenderer>().transform.localScale,
                oRotate = GetComponentInChildren<MeshRenderer>().transform.localRotation.eulerAngles;
            GetComponentInChildren<MeshRenderer>().transform.localScale = new Vector3(oScale.x, oScale.y, -oScale.z);
            GetComponentInChildren<MeshRenderer>().transform.localRotation = Quaternion.Euler(new Vector3(-oRotate.x, oRotate.y, oRotate.z));
        }
    }
}
