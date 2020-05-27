using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for the User propreties in VR.
/// </summary>
public class VRUserController : MonoBehaviour
{
    /// <summary>
    /// Main, and only, instance.
    /// </summary>
    public static VRUserController main;

    /// <summary>
    /// Saved next movement for the Character Controller.
    /// </summary>
    public Vector3 nextMovement;
    /// <summary>
    /// Character Controller of the current, and only, User.
    /// </summary>
    private CharacterController charController;

    /// <summary>
    /// Awake Function, search for the Character Controller.
    /// </summary>
    private void Awake()
    {
        main = this;
        charController = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Update Function, move Character and reset the next movement.
    /// </summary>
    private void Update()
    {
        charController.Move(nextMovement);
        nextMovement = Vector3.zero;
    }
}
