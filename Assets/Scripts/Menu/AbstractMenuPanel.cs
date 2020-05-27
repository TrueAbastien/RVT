using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for menu prototyping.
/// </summary>
public abstract class AbstractMenuPanel : MonoBehaviour
{
    /// <summary>
    /// Abstract function to call on selection of the specific Menu Panel.
    /// </summary>
    public abstract void handle();
}
