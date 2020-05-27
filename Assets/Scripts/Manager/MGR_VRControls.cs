using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager for the VR Controls of the Oculus Rift.
/// Allow user to access and define which hand to be primary or secondary (and more).
/// </summary>
public class MGR_VRControls : MonoBehaviour
{
    /// <summary>
    /// Primary Hand define (Right or Left).
    /// </summary>
    public static VRHand.Hand mainHand;
    /// <summary>
    /// Right hand game object controller script.
    /// </summary>
    public VRHandController RightHand;
    /// <summary>
    /// Left hand game object controller script.
    /// </summary>
    public VRHandController LeftHand;

    /// <summary>
    /// Static Manager instance.
    /// </summary>
    private static MGR_VRControls Instance = null;

    /// <summary>
    /// Getter for the Static Manager instance.
    /// </summary>
    public static MGR_VRControls get { get { return Instance; } }

    /// <summary>
    /// Awake function, set instance and compute main hand.
    /// </summary>
    private void Awake()
    {
        Instance = this;
        set(mainHand);
    }


    // --------------------------------------- //

    /// <summary>
    /// Hold Controls data for the whole input system. 
    /// </summary>
    private VRHand.Controls mainInput;

    /// <summary>
    /// Getter for the main input control data.
    /// </summary>
    public VRHand.Controls current { get { return mainInput; } }

    /// <summary>
    /// Getter for the Right hand input data.
    /// </summary>
    public VRHand.Controls.HandInput right { get { return mainInput.content[(int)VRHand.Hand.RIGHT]; } }

    /// <summary>
    /// Getter for the Left hand input data.
    /// </summary>
    public VRHand.Controls.HandInput left { get { return mainInput.content[(int)VRHand.Hand.LEFT]; } }

    /// <summary>
    /// Access on a specific hand input data.
    /// </summary>
    /// <param name="_hand">Specific hand to access from</param>
    /// <returns>Hand input data</returns>
    public VRHand.Controls.HandInput hand(VRHand.Hand _hand) { return mainInput.content[(int)_hand]; }

    /// <summary>
    /// Getter for the Primary hand scene object.
    /// </summary>
    public VRHandController primary { get { return mainHand == VRHand.Hand.RIGHT ? RightHand : LeftHand; } }

    /// <summary>
    /// Getter for the Secondary hand scene object.
    /// </summary>
    public VRHandController secondary { get { return mainHand == VRHand.Hand.LEFT ? RightHand : LeftHand; } }

    /// <summary>
    /// Access a specific hand scene object. 
    /// </summary>
    /// <param name="_hand">Specific hand to access from</param>
    /// <returns>Hand scene object</returns>
    public VRHandController handObject(VRHand.Hand _hand) { return _hand == VRHand.Hand.RIGHT ? RightHand : LeftHand; }

    /// <summary>
    /// Swap the main hand if necessary.
    /// </summary>
    public void swap() { VRHand.reverseHand(ref mainInput); }

    /// <summary>
    /// Set the main input Controls depending on the main hand.
    /// </summary>
    /// <param name="mainHand">Specific main hand</param>
    public void set(VRHand.Hand mainHand) { mainInput = VRHand.constructOver(mainHand); }
}
