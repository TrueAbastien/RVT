using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for VR Item Handling (from Inventory).
/// </summary>
public abstract class InventoryItem : MonoBehaviour
{
    /// <summary>
    /// Currently used Hand for the selected item.
    /// </summary>
    protected VRHand.Controls.HandInput currentInput;

    /// <summary>
    /// Allow for object selection of a new Item.
    /// </summary>
    /// <param name="go">Item object prefab</param>
    /// <param name="target">Script for targetted hand object</param>
    /// <param name="hand">Specific Hand for Input (Right or Left)</param>
    public static void Select(GameObject go, VRHandController target, VRHand.Hand hand)
    {
        if (go == null)
            return;

        if (target.GetComponentInChildren<InventoryItem>() != null)
            Destroy(target.GetComponentInChildren<InventoryItem>().gameObject);

        GameObject obj = Instantiate(go, target.transform);
        obj.GetComponent<InventoryItem>().onCreation(target, hand);
    }

    /// <summary>
    /// Called on Item selection, allow current item to be modified accordingly.
    /// </summary>
    /// <param name="target">Script for targetted hand object</param>
    /// <param name="hand">Specific Hand for Input (Right or Left)</param>
    public void onCreation(VRHandController target, VRHand.Hand hand)
    {
        currentInput = MGR_VRControls.get.hand(hand);

        //transform.localPosition = Vector3.zero;
        GetComponentInChildren<MeshRenderer>().sharedMaterial = target.handMaterial;

        target.ChangeTo(gameObject);
        ConstructOver(hand);
    }

    /// <summary>
    /// Abstract function for Item selection handling (called on selection).
    /// </summary>
    /// <param name="hand">Specific Hand for Input (Right or Left)</param>
    protected abstract void ConstructOver(VRHand.Hand hand);
}
