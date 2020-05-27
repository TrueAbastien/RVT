using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script calling the current SceneInit to intialize the current 'View Scene'.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Parent for every composing Object.
    /// </summary>
    public GameObject Terrain;
    /// <summary>
    /// Parent for the User/Player prefab Object.
    /// </summary>
    public GameObject User;

    /// <summary>
    /// Awake Function, load the current scene through its data.
    /// </summary>
    void Awake()
    {
        // Load Scene
        SceneInit.current.onSceneLoaded = delegate 
        {
            LoadSettings();
            Destroy(SceneInit.current.gameObject);
        };
        SceneInit.GlobalLoad(User, Terrain);
    }

    /// <summary>
    /// Load & apply every first-frame settings for the 'View Scene'.
    /// </summary>
    private void LoadSettings()
    {
        MGR_VRControls.mainHand = SceneInit.current.settings.MainHand;
    }
}
