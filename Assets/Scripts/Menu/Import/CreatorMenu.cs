using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Creation menu for quick import on PolygonLoader side.
/// </summary>
public class CreatorMenu : MonoBehaviour
{
    /// <summary>
    /// Input field for the import Key.
    /// </summary>
    public TMP_InputField input;

    /// <summary>
    /// Load Function from specified Input field.
    /// </summary>
    public void LoadFromInput()
    {
        PolygonLoader.Load(input.text);
    }
}
