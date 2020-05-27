using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switch the current Camera (black screen fading).
/// </summary>
public class CameraSwitchController : MonoBehaviour
{
    /// <summary>
    /// List of all existing Cameras.
    /// </summary>
    public List<Camera> AllCams = null;
    /// <summary>
    /// Primary camera Index.
    /// </summary>
    private int currIndex = 0;

    /// <summary>
    /// Button for switching Camera.
    /// </summary>
    public VRButton.Code switchButton;


    /// <summary>
    /// Update Function, change the Camera to the next on the list.
    /// </summary>
    private void Update()
    {
        if (VRButton.onPressed(switchButton) && AllCams != null)
        {
            if (MGR_Camera.ChangeCamera(AllCams[(currIndex + 1) % AllCams.Count]))
                currIndex = ++currIndex % AllCams.Count;
        }
    }
}
