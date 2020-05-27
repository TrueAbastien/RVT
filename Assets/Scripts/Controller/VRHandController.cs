using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hand controller for any current hand (Right or Left).
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class VRHandController : MonoBehaviour, Listener
{
    /// <summary>
    /// Current hand side.
    /// </summary>
    public VRHand.Hand hand;
    /// <summary>
    /// Current hand color.
    /// </summary>
    public Color handColor;
    /// <summary>
    /// Current hand material.
    /// </summary>
    public Material handMaterial;

    /// <summary>
    /// Darkening multiplier on Grip.
    /// </summary>
    public float darkening = .5f;
    /// <summary>
    /// Scale multiplier on maximum Grip.
    /// </summary>
    public float tightening = .7f;
    /// <summary>
    /// Speed at which the Grip trigger.
    /// </summary>
    public float grabSpeed = 3f;

    /// <summary>
    /// Body of the hand.
    /// </summary>
    private Transform handBody;

    /// <summary>
    /// Scale on Start.
    /// </summary>
    private float initScale;

    /// <summary>
    /// Current pressure value of Grip.
    /// </summary>
    private float CurrentPressure;

    /// <summary>
    /// Audio source for hand.
    /// </summary>
    private AudioSource source;

    /// <summary>
    /// Verify if the instance has been late-started.
    /// </summary>
    private bool tRunning = false;

    /// <summary>
    /// Update Event Function, trigger source on Enter.
    /// (Isn't currently working nor usefull).
    /// </summary>
    /// <param name="evt">String trigger object</param>
    public void update(string evt)
    {
        switch (evt)
        {
            case "onEnter":
                source.Play();
                break;
            case "onStay":
                break;
            case "onExit":
                break;
            default: break;
        }
    }

    /// <summary>
    /// Late Start Function, initialize propreties and set object values.
    /// </summary>
    public void LateStart()
    {
        ChangeTo(GetComponentInChildren<InventoryItem>().gameObject);

        handMaterial.color = handColor;
        source = GetComponent<AudioSource>();
        tRunning = true;
    }

    /// <summary>
    /// Change physic object propreties.
    /// </summary>
    /// <param name="child">Child object</param>
    public void ChangeTo(GameObject child)
    {
        handBody = child.transform;
        initScale = handBody.localScale.x;
        CurrentPressure = 0;
    }

    /// <summary>
    /// Update Function, update propreties depending on Grip/pressure.
    /// </summary>
    void Update()
    {
        if (!tRunning)
            return;

        CurrentPressure = Mathf.Lerp(CurrentPressure, Input.GetAxis(MGR_VRControls.get.hand(hand).Middle), grabSpeed * Time.deltaTime);

        handBody.localScale = Vector3.one * (initScale - initScale * (1f - tightening) * CurrentPressure);
        handMaterial.color = handColor - handColor * (1f - darkening) * CurrentPressure;
    }
}
