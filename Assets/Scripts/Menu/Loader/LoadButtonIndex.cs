using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Index for a Load Button.
/// </summary>
[RequireComponent(typeof(Button))]
public class LoadButtonIndex : MonoBehaviour
{
    /// <summary>
    /// Index value for the current Load Button.
    /// </summary>
    [HideInInspector] public int value = -1;
    /// <summary>
    /// Reference for the, unique, Loader Panel.
    /// </summary>
    [HideInInspector] public LoaderMenuPanel loader = null;

    /// <summary>
    /// Press the current Load Button.
    /// Called by button press Event.
    /// </summary>
    public void Trigger()
    {
        if (loader != null)
            loader.LoadFile(this);
    }
}
