using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inventory Item, controller for the Drone Mouvements
/// </summary>
public class VRDroneMovementController : InventoryItem
{
    /// <summary>
    /// Sensitivity of drone rotation: horizontally or vertically.
    /// </summary>
    public Vector2 sensitivity = new Vector2(.8f, .5f);
    /// <summary>
    /// Multiplier in drone translate-movements.
    /// </summary>
    public float thrust = 1f;

    /// <summary>
    /// Instance of the Drone Controller for drone transform movements.
    /// </summary>
    private DroneController instance;

    /// <summary>
    /// Start Function, init propreties turn the motors on.
    /// </summary>
    void Start()
    {
        instance = DroneController.drone;

        instance.turnOn(true);
    }

    /// <summary>
    /// Update function, update each and every controls depending on Inputs.
    /// </summary>
    void Update()
    {
        instance.setTorque(Input.GetAxis(currentInput.ThumbX) * sensitivity.x, DroneController.Axis.Y);
        instance.setTorque(Input.GetAxis(currentInput.ThumbY) * sensitivity.y, DroneController.Axis.X);

        instance.setVelocity((Input.GetAxis(currentInput.Index) - Input.GetAxis(currentInput.Middle)) * thrust, DroneController.Axis.Y);
    }

    /// <summary>
    /// OnDestroy Function, turn the motors off.
    /// </summary>
    private void OnDestroy()
    {
        instance.turnOn(false);
    }

    /// <summary>
    /// Empty overwritten method.
    /// </summary>
    /// <param name="hand">Current item's hand</param>
    protected override void ConstructOver(VRHand.Hand hand) { }
}
