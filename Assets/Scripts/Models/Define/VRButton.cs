using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Common class, part of VR Input define.
/// Allow us to handle Top-Buttons on a Occulus Rift controller.
/// </summary>
public class VRButton
{
    /// <summary>
    /// Names of all four buttons.
    /// </summary>
    private static string[] __names =
    {
        "VRButtonA",
        "VRButtonB",
        "VRButtonX",
        "VRButtonY"
    };

    /// <summary>
    /// Enumeration of each existing button.
    /// </summary>
    public enum Code
    {
        A = 0,
        B = 1,
        X = 2,
        Y = 3
    }

    /// <summary>
    /// Conversion function to access Input Axis Name.
    /// </summary>
    /// <param name="k">Button code</param>
    /// <returns>Name of the Input Axis</returns>
    public static string toString(Code k)
    {
        return __names[(int)k];
    }

    /// <summary>
    /// Quicker way to handle 'GetButtonDown' function for a Button.
    /// </summary>
    /// <param name="k">Button code</param>
    /// <returns>Result of the 'GetButtonDown' function</returns>
    public static bool onPressed(Code k)
    {
        return Input.GetButtonDown(toString(k));
    }
}


