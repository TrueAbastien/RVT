using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VRHandSensor : MonoBehaviour
{
    //public List<LayerMask> reactingLayers = null;
    private VRHandController parentHand;
    private EventLauncher onEnter, onStay, onExit;

    // Start Function
    void Start()
    {
        parentHand = transform.parent.GetComponent<VRHandController>();
        onEnter = new EventLauncher("onEnter", parentHand);
        onStay = new EventLauncher("onStay", parentHand);
        onExit = new EventLauncher("onExit", parentHand);
    }

    // On Collision Enter Function
    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (reactingLayers != null)
            foreach (var lay in reactingLayers)
                if ((lay.value & 1<<collision.gameObject.layer) != 0)
                {
                    onEnter.notify();
                    return;
                }
                */
        onEnter.notify();
    }

    // On Collision Stay Function
    private void OnCollisionStay(Collision collision)
    {
        onStay.notify();
    }

    // On Collision Exit Function
    private void OnCollisionExit(Collision collision)
    {
        onExit.notify();
    }
}
