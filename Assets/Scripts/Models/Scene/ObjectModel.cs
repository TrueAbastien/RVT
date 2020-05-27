using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object data for Model Input.
/// </summary>
public class ObjectModel : MonoBehaviour
{
    /// <summary>
    /// Current Model key for import.
    /// </summary>
    public string key = null;

    /// <summary>
    /// Get the current Object as ModelData.
    /// </summary>
    /// <returns>Result of the transformation</returns>
    public SceneData.ModelData get()
    {
        return new SceneData.ModelData(key, transform);
    }
}
