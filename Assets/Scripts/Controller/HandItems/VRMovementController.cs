using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inventory Item, controls the User body Movements.
/// </summary>
public class VRMovementController : InventoryItem
{
    /// <summary>
    /// Movement speed.
    /// </summary>
    public Vector2 Speed = new Vector2(4f, 5f);
    /// <summary>
    /// Force of gravity for the User body.
    /// </summary>
    public float Gravity = 40f;
    /// <summary>
    /// Jump boost strength.
    /// </summary>
    public float Jump = 10f;
    /// <summary>
    /// Sprint multiplier (when triggered).
    /// </summary>
    public float SprintMultiplier = 1.5f;
    //private Vector2 BobbingForce = new Vector2(12e-2f, 8e-2f);
    //private Vector2 BobbingSpeed = new Vector2(5f, 10f);
    /// <summary>
    /// Jump button sensitivity for it to trigger (between 0 and 1).
    /// </summary>
    public float jumpSensibility = .4f;

    /// <summary>
    /// User character controller.
    /// </summary>
    private CharacterController charController;
    /// <summary>
    /// Current movement direction. 
    /// </summary>
    private Vector3 moveDirection;
    /// <summary>
    /// Current user camera.
    /// </summary>
    private Camera UserCam;

    //private float headBobbingTimer;


    /// <summary>
    /// Start Function, initialize propreties.
    /// </summary>
    void Start()
    {
        charController = VRUserController.main.gameObject.GetComponent<CharacterController>();
        UserCam = Camera.main;
    }

    /// <summary>
    /// Update Function, move user accordingly to Inputs.
    /// </summary>
    void Update()
    {
        if (charController.isGrounded)
        {
            moveDirection = Input.GetAxis(currentInput.ThumbX) * UserCam.transform.right * Speed.x +
                Input.GetAxis(currentInput.ThumbY) * UserCam.transform.forward * Speed.y;
            moveDirection.y = 0f;

            // Sprint
            moveDirection *= SprintMultiplier * Input.GetAxis(currentInput.Middle) + 1;

            // Jump
            if (Input.GetAxis(currentInput.Index) > jumpSensibility)
            {
                moveDirection.y = Jump;
            }

            // Head Bobbing
            //headBobbingTimer += Time.deltaTime * new Vector2(Input.GetAxis("Horizontal"),
            //    Input.GetAxis("Vertical")).sqrMagnitude * (Input.GetKey(KeyCode.LeftShift) ? SprintMultiplier : 1f);
            //ApplyHeadBobbing();
        }
        moveDirection.y -= Gravity * Time.deltaTime;
        VRUserController.main.nextMovement = moveDirection * Time.deltaTime;
    }

    // Apply Head Bobbing Effect on User Camera
    //private void ApplyHeadBobbing()
    //{
    //    UserCam.transform.localPosition = new Vector3(BobbingForce.x * Mathf.Sin(headBobbingTimer * BobbingSpeed.x),
    //        BobbingForce.y * Mathf.Cos(headBobbingTimer * BobbingSpeed.y), 0f);
    //}

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
