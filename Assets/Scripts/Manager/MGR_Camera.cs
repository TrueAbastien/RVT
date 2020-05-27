using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manager for the current Camera selection.
/// Meant for Camera transition (which is actually not fully-implemented but used to be flawed).
/// </summary>
public class MGR_Camera : MonoBehaviour
{
    /// <summary>
    /// Fading speed before transition.
    /// </summary>
    private float fadingSpeed = 1.5f;
    /// <summary>
    /// Appearing speed after transition.
    /// </summary>
    private float appearingSpeed = 1.2f;

    /// <summary>
    /// Static Manager instance.
    /// </summary>
    private static MGR_Camera Instance = null;
    /// <summary>
    /// Previously selected Camera.
    /// </summary>
    [HideInInspector] public Camera previousCam = null;
    /// <summary>
    /// Currently selected Camera.
    /// </summary>
    [HideInInspector] public Camera currentCam = null;

    /// <summary>
    /// Mask canvas meant for black shading the screeen on transition.
    /// </summary>
    private Canvas maskCameraCanvas;
    /// <summary>
    /// Panel containing the black image to shade with.
    /// </summary>
    private Image maskPanel;
    /// <summary>
    /// Current level of opacity of the 'maskPanel' image color.
    /// </summary>
    [HideInInspector] public float currentTransparency;

    /// <summary>
    /// Temporary value allowing static call to generate quick transition reset.
    /// </summary>
    [HideInInspector] public float tValue;


    /// <summary>
    /// Start function, initilize each propreties and move the current canvas to the current Camera.
    /// </summary>
    private void Start()
    {
        Instance = this;
        currentCam = Camera.main;
        maskCameraCanvas = GetComponentInChildren<Canvas>();
        maskPanel = maskCameraCanvas.gameObject.GetComponentInChildren<Image>();
        currentTransparency = 0f;

        MoveCanvasTo(currentCam);
        maskPanel.color = new Color(0, 0, 0, currentTransparency);
    }

    /// <summary>
    /// Update function, compute Fading & Appearing effects.
    /// </summary>
    private void Update()
    {
        // Fading
        if (previousCam != null)
        {
            if (currentTransparency > 1f)
                Switch();
            else
            {
                tValue = Time.deltaTime / fadingSpeed;
                currentTransparency += tValue;
                maskPanel.color += new Color(0, 0, 0, tValue);
            }
        }

        // Appearing
        else if (currentTransparency > 0f)
        {
            tValue = Time.deltaTime / appearingSpeed;
            currentTransparency -= tValue;
            maskPanel.color -= new Color(0, 0, 0, tValue);
        }
    }

    /// <summary>
    /// Switch the main camera to the currently selected one.
    /// </summary>
    private void Switch()
    {
        currentCam.enabled = true;
        MoveCanvasTo(currentCam);
        previousCam.enabled = false;
        previousCam = null;
        maskCameraCanvas.worldCamera = currentCam;
    }

    /// <summary>
    /// Change main camera to a specified one.
    /// </summary>
    /// <param name="cam">Specific camera</param>
    /// <returns>Verify that a transition is not currently running</returns>
    public static bool ChangeCamera(Camera cam)
    {
        if (Instance.currentTransparency > 0f)
            return false;
        Instance.previousCam = Instance.currentCam;
        Instance.currentCam = cam;
        Instance.tValue = 0f;
        return true;
    }

    /// <summary>
    /// Move 'maskCameraCanvas' to a new Camera as Parent.
    /// </summary>
    /// <param name="_cam">New Camera parent</param>
    private void MoveCanvasTo(Camera _cam)
    {
        maskCameraCanvas.transform.SetParent(_cam.transform);
        maskCameraCanvas.transform.localPosition = new Vector3(0, 0, _cam.nearClipPlane + 1e-1f);
        maskCameraCanvas.transform.localRotation = Quaternion.Euler(0, 0, 0);
        maskCameraCanvas.transform.localScale = Vector3.one;
    }

}
