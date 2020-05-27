using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using PolyToolkit;

/// <summary>
/// Scene Initializer, used to create the objects composing a 'View Scene'.
/// </summary>
public class SceneInit : MonoBehaviour
{
    /// <summary>
    /// Current script instance.
    /// </summary>
    public static SceneInit current = null;

    /// <summary>
    /// Prefab of the player to instantiate.
    /// </summary>
    public GameObject playerPrefab;
    /// <summary>
    /// Multiplier of the stored player scale to the prefab scale.
    /// </summary>
    private float ObjectToPlayerRatio = 2f;

    /// <summary>
    /// Data generating the 'View Scene'.
    /// </summary>
    private SceneData data;
    /// <summary>
    /// Settings specified for the 'View Scene' on creation.
    /// </summary>
    [HideInInspector] public SettingsMenuPanel.SettingData settings;

    /// <summary>
    /// Delegation template, simple method.
    /// </summary>
    public delegate void LoadCallback();
    /// <summary>
    /// Delegated method, called when the scene got successfuly created.
    /// </summary>
    public LoadCallback onSceneLoaded;

    /// <summary>
    /// Awake Function, preserve object and initialize basic propreties as soon as possible.
    /// </summary>
    private void Awake()
    {
        current = this;
        onAwaken();

        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// Store any data meant for the construction of the 'View Scene'.
    /// Called on 'Creation Scene' destruction.
    /// </summary>
    /// <param name="_data">Scene data of the scene to load/construct</param>
    public void Save(SceneData _data)
    {
        data = _data;
        settings = SettingsMenuPanel.data;

        // Pre Settings
        ObjectToPlayerRatio = settings.UserSize;
    }

    // ----------------------------------------------------------------------------------------- //

    /// <summary>
    /// Counter for asynchronous object loading.
    /// </summary>
    private int counter = 0;
    /// <summary>
    /// Model currently being loaded.
    /// </summary>
    private SceneData.ModelData currentlyLoading;

    /// <summary>
    /// Object storing the Player as child.
    /// </summary>
    private GameObject playerParent;
    /// <summary>
    /// Object storing any Object as child.
    /// </summary>
    private GameObject objectParent;

    /// <summary>
    /// Load the current scene from any point.
    /// Called by a Loader Script on 'View Scene' creation.
    /// </summary>
    /// <param name="_playerParent">Parent for the player object</param>
    /// <param name="_objectParent">Parent for any scene object</param>
    public static void GlobalLoad(GameObject _playerParent, GameObject _objectParent)
    {
        if (current != null)
            current.Load(_playerParent, _objectParent);
    }

    /// <summary>
    /// Load the current scene by setting propreties and loading the first object.
    /// </summary>
    /// <param name="_playerParent">Parent for the player object</param>
    /// <param name="_objectParent">Parent for any scene object</param>
    public void Load(GameObject _playerParent, GameObject _objectParent)
    {
        if (counter > 0)
            return;

        playerParent = _playerParent;
        objectParent = _objectParent;

        counter = 0;
        LoadNext();
    }

    /// <summary>
    /// Load the next object stored in data.
    /// If every object has been loaded, instantiate Player.
    /// </summary>
    private void LoadNext()
    {
        // Create Objects
        if (counter < data.models.Count)
        {
            currentlyLoading = data.models[counter];
            PolyApi.GetAsset("assets/" + data.models[counter].key, GetAssetCallback);
            counter++;
        }

        // Create Player
        else
        {
            Instantiate(playerPrefab, data.PlayerPosition.get(),
                Quaternion.Euler(0, data.PlayerEulerRotation.y, 0), playerParent.transform);
            counter = 0;

            onSuccessfulLoad();
        }
    }

    /// <summary>
    /// Callback for Poly asset GET call.
    /// </summary>
    /// <param name="result">Result of the GET call</param>
    private void GetAssetCallback(PolyStatusOr<PolyAsset> result)
    {
        if (!result.Ok)
        {
            return;
        }

        PolyImportOptions options = PolyImportOptions.Default();
        options.rescalingMode = PolyImportOptions.RescalingMode.FIT;
        options.desiredSize = 1.0f;
        options.recenter = true;

        PolyApi.Import(result.Value, options, ImportAssetCallback);
    }

    /// <summary>
    /// Callback for a model Importation.
    /// Called on successful load.
    /// </summary>
    /// <param name="asset">Current asset</param>
    /// <param name="result">Result of the Importation</param>
    public void ImportAssetCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result)
    {
        if (!result.Ok)
        {
            return;
        }

        ColliderMaker.GenerateOn(result.Value.gameObject, true);
        Transform obj = result.Value.gameObject.transform;
        obj.position = currentlyLoading.position.get();
        obj.rotation = Quaternion.Euler(currentlyLoading.eulerRotation.get());
        obj.localScale = currentlyLoading.scale.get() / data.PlayerScale.x / ObjectToPlayerRatio;
        obj.SetParent(objectParent.transform);

        LoadNext();
    }

    /// <summary>
    /// Method called in 'Awake' function.
    /// Meant to be called on 'Construction Scene' destruction.
    /// </summary>
    private void onAwaken()
    {
        if (GetComponent<Camera>() == null)
            gameObject.AddComponent<Camera>();
    }

    /// <summary>
    /// Method called after a successful load.
    /// Meant for be called on 'View Scene' creation.
    /// </summary>
    private void onSuccessfulLoad()
    {
        if (GetComponent<Camera>() != null)
            Destroy(GetComponent<Camera>());

        onSceneLoaded();
    }
}
