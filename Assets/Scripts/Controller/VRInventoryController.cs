using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for the User Inventory in VR.
/// </summary>
public class VRInventoryController : MonoBehaviour
{
    /// <summary>
    /// Hand for inventory.
    /// </summary>
    private VRHand.Hand InventoryHand;
    /// <summary>
    /// Turn inventory canvas on and off button.
    /// </summary>
    private VRButton.Code SwitchButton;
    /// <summary>
    /// Cycle Right hand targetted Item in Inventory.
    /// </summary>
    public VRButton.Code CycleRightHand;
    /// <summary>
    /// Cycle Left hand targetted Item in Inventory.
    /// </summary>
    public VRButton.Code CycleLeftHand;

    /// <summary>
    /// Activation state of the inventory object/canvas.
    /// </summary>
    private bool CurrentInventoryState = false;

    /// <summary>
    /// Left hand current item index.
    /// </summary>
    private int LeftHandIndex;
    /// <summary>
    /// Right hand current item index.
    /// </summary>
    private int RightHandIndex;
    /// <summary>
    /// Saved previous Left hand index.
    /// </summary>
    private int saved_LeftIndex = -1;
    /// <summary>
    /// Saved previous Right hand index.
    /// </summary>
    private int saved_RightIndex = -1;

    /// <summary>
    /// Temporary inventory size.
    /// </summary>
    private int tInvSize;
    /// <summary>
    /// Current inventory generator instance.
    /// </summary>
    private InventoryGenerator generatorInstance;


    /// <summary>
    /// Start Function, initialize propreties, and inputs, and make first selection.
    /// </summary>
    void Start()
    {
        generatorInstance = GetComponentInChildren<InventoryGenerator>();

        InventoryHand = MGR_VRControls.mainHand;
        SwitchButton = (InventoryHand == VRHand.Hand.RIGHT) ? VRButton.Code.B : VRButton.Code.Y;

        transform.SetParent(InventoryHand == VRHand.Hand.RIGHT ?
            MGR_VRControls.get.RightHand.transform : MGR_VRControls.get.LeftHand.transform);
        transform.localPosition = new Vector3(0, .25f, 0);
        generatorInstance.gameObject.SetActive(CurrentInventoryState);

        tInvSize = generatorInstance.invContent.Count;

        // First Select
        switch (MGR_VRControls.mainHand)
        {
            case VRHand.Hand.LEFT:
                LeftHandIndex = 0;
                RightHandIndex = 1;
                break;
            case VRHand.Hand.RIGHT:
                RightHandIndex = 0;
                LeftHandIndex = 1;
                break;
            default: break;
        }
        ComputeSelection(MGR_VRControls.get.RightHand, MGR_VRControls.get.LeftHand);

        generatorInstance.holdOn(0, MGR_VRControls.get.primary.handColor);
        generatorInstance.holdOn(1, MGR_VRControls.get.secondary.handColor);

        MGR_VRControls.get.primary.LateStart();
        MGR_VRControls.get.secondary.LateStart();
    }

    /// <summary>
    /// Update Function, process input by cycling through items or changing the activation state of the Canvas.
    /// </summary>
    void Update()
    {
        // Turn Inventory On & Off
        if (VRButton.onPressed(SwitchButton))
        {
            generatorInstance.gameObject.SetActive(CurrentInventoryState = !CurrentInventoryState);
            if (CurrentInventoryState)
                SaveHandIndexes();
            else ComputeSelection(MGR_VRControls.get.RightHand, MGR_VRControls.get.LeftHand);
        }

        // Cycle Through Items
        if (CurrentInventoryState)
        {
            if (VRButton.onPressed(CycleLeftHand))
                CycleItem(VRHand.Hand.LEFT);
            if (VRButton.onPressed(CycleRightHand))
                CycleItem(VRHand.Hand.RIGHT);
        }
    }

    /// <summary>
    /// Cycle item by changing its color.
    /// Select item when the inventory is desactivated.
    /// </summary>
    /// <param name="hand">Current hand to cycle with</param>
    private void CycleItem(VRHand.Hand hand)
    {
        if (generatorInstance.inventory.Count == 0)
            return;

        switch (hand)
        {
            case VRHand.Hand.RIGHT:
                generatorInstance.invContent[RightHandIndex][0].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                generatorInstance.invContent[RightHandIndex][0].transform.GetChild(0).GetComponent<Image>().sprite = generatorInstance.emptyImage;

                RightHandIndex++;
                if (RightHandIndex % tInvSize == LeftHandIndex)
                    RightHandIndex++;
                RightHandIndex %= tInvSize;

                generatorInstance.invContent[RightHandIndex][0].transform.GetChild(0).GetComponent<Image>().color = MGR_VRControls.get.RightHand.handColor;
                generatorInstance.invContent[RightHandIndex][0].transform.GetChild(0).GetComponent<Image>().sprite = generatorInstance.holdingImage;
                break;

            case VRHand.Hand.LEFT:
                generatorInstance.invContent[LeftHandIndex][0].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                generatorInstance.invContent[LeftHandIndex][0].transform.GetChild(0).GetComponent<Image>().sprite = generatorInstance.emptyImage;

                LeftHandIndex++;
                if (LeftHandIndex % tInvSize == RightHandIndex)
                    LeftHandIndex++;
                LeftHandIndex %= tInvSize;

                generatorInstance.invContent[LeftHandIndex][0].transform.GetChild(0).GetComponent<Image>().color = MGR_VRControls.get.LeftHand.handColor;
                generatorInstance.invContent[LeftHandIndex][0].transform.GetChild(0).GetComponent<Image>().sprite = generatorInstance.holdingImage;
                break;
            default: return;
        }
    }

    /// <summary>
    /// Save the current hand Indexes (as previous).
    /// </summary>
    private void SaveHandIndexes()
    {
        saved_LeftIndex = LeftHandIndex;
        saved_RightIndex = RightHandIndex;
    }

    /// <summary>
    /// Select items currently in hand.
    /// Called on closing inventory.
    /// </summary>
    /// <param name="RHand">Right hand controller instance</param>
    /// <param name="LHand">Left hand controller instance</param>
    private void ComputeSelection(VRHandController RHand, VRHandController LHand)
    {
        // Left Hand
        if (generatorInstance.invContent[LeftHandIndex][1] == null)
        {
            LeftHandIndex = saved_LeftIndex;
        }
        else if (saved_LeftIndex != LeftHandIndex)
        {
            InventoryItem.Select(generatorInstance.invContent[LeftHandIndex][1], LHand, VRHand.Hand.LEFT);
        }

        // Right Hand
        if (generatorInstance.invContent[RightHandIndex][1] == null)
        {
            RightHandIndex = saved_RightIndex;
        }
        else if (saved_RightIndex != RightHandIndex)
        {
            InventoryItem.Select(generatorInstance.invContent[RightHandIndex][1], RHand, VRHand.Hand.RIGHT);
        }

        SaveHandIndexes();
    }
}
