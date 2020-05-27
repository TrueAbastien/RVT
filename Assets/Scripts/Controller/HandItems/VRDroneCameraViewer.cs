using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inventory Item, advanced drone controller if the Remote is equiped in the other hand.
/// </summary>
public class VRDroneCameraViewer : InventoryItem
{
    /// <summary>
    /// Drone controller unique instance.
    /// </summary>
    private DroneController instance;

    /// <summary>
    /// Horizontal local speeds of the drone movements.
    /// X value is Right while Y value is Forward.
    /// </summary>
    public Vector2 horizontalSpeeds = new Vector2(.7f, .8f);
    /// <summary>
    /// Sensitivity triggering the block Buttons.
    /// </summary>
    [Range(0f, 1f)] public float blockSensitivity = .4f;

    /// <summary>
    /// Start Function, initialize the current drone instance.
    /// </summary>
    void Start()
    {
        instance = DroneController.drone;
    }

    /// <summary>
    /// Update Function, used to move the drone horizontally if 'activated'.
    /// </summary>
    void Update()
    {
        // Cannot update if drone isn't active
        if (!instance.running)
            return;

        // Horizontal Movements
        instance.setVelocity(Input.GetAxis(currentInput.ThumbX) * horizontalSpeeds.x, DroneController.Axis.X);
        instance.setVelocity(Input.GetAxis(currentInput.ThumbY) * horizontalSpeeds.y, DroneController.Axis.Z);
    }

    /// <summary>
    /// LateUpdate Function, used to block Roll and Yaw from the DroneMovementController (Remote) if 'activated'.
    /// </summary>
    private void LateUpdate()
    {
        // Cannot update if drone isn't active
        if (!instance.running)
            return;

        // Block Roll
        if (Input.GetAxis(currentInput.Middle) > blockSensitivity)
            instance.setTorque(0, DroneController.Axis.X);

        // Block Yaw
        if (Input.GetAxis(currentInput.Index) > blockSensitivity)
            instance.setTorque(0, DroneController.Axis.Y);
    }

    /// <summary>
    /// Empty overwritten method.
    /// </summary>
    /// <param name="hand">Current item's hand</param>
    protected override void ConstructOver(VRHand.Hand hand) { }
}
