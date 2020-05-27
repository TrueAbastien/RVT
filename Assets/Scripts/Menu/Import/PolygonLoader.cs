using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PolyToolkit;

/// <summary>
/// Model loader for Google Poly.
/// </summary>
public class PolygonLoader : MonoBehaviour
{
    /// <summary>
    /// Field for Credit information.
    /// </summary>
    public TextMeshProUGUI CreditField;
    /// <summary>
    /// Field for Status state of importation.
    /// </summary>
    public TextMeshProUGUI StatusField;
    /// <summary>
    /// Preview object for model display.
    /// </summary>
    public Transform PreviewEmptyObject;

    /// <summary>
    /// Current PolygonLoader instance.
    /// </summary>
    private static PolygonLoader instance;
    /// <summary>
    /// Indicate if a model is actually being imported.
    /// </summary>
    [HideInInspector] public bool isRunning;

    /// <summary>
    /// Store the latest successful loaded Asset key specified through Input Field.
    /// </summary>
    public static string latestKey = null;


    /// <summary>
    /// Awake Function, basic initialization.
    /// </summary>
    void Awake()
    {
        instance = this;

        isRunning = false;

        CreditField.text = "";
        StatusField.text = "";
    }

    /// <summary>
    /// Change preview activate state.
    /// </summary>
    /// <param name="state">New specific state</param>
    public static void SetPreviewActive(bool state)
    {
        instance.PreviewEmptyObject.gameObject.SetActive(state);
        instance.CreditField.gameObject.SetActive(state);
    }

    /// <summary>
    /// Load asset from a specific key string.
    /// </summary>
    /// <param name="key">Specific key (string)</param>
    public static void Load(string key)
    {
        if (instance.isRunning)
            return;

        instance.StatusField.gameObject.SetActive(true);
        instance.isRunning = true;

        latestKey = key;

        PolyApi.GetAsset("assets/" + key, instance.GetAssetCallback);
        instance.StatusField.text = "Requesting...";
    }

    /// <summary>
    /// Specified callback on GET Asset.
    /// </summary>
    /// <param name="result">Result of the GET method</param>
    public void GetAssetCallback(PolyStatusOr<PolyAsset> result)
    {
        if (!result.Ok)
        {
            StatusField.text = "ERROR: " + result.Status;
            isRunning = false;
            return;
        }

        PolyImportOptions options = PolyImportOptions.Default();
        options.rescalingMode = PolyImportOptions.RescalingMode.FIT;
        options.desiredSize = 5.0f;
        options.recenter = true;

        StatusField.text = "Importing...";
        PolyApi.Import(result.Value, options, ImportAssetCallback);
    }

    /// <summary>
    /// Callback for model importing.
    /// </summary>
    /// <param name="asset">Imported asset</param>
    /// <param name="result">Result of the import</param>
    public void ImportAssetCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result)
    {
        isRunning = false;

        if (!result.Ok)
        {
            StatusField.text = "ERROR: Import failed: " + result.Status;
            latestKey = null;
            return;
        }

        instance.StatusField.gameObject.SetActive(false);
        CreditField.text = asset.displayName + "\nby " + asset.authorName;

        if (PreviewEmptyObject.childCount > 0)
            Destroy(PreviewEmptyObject.GetChild(0).gameObject);

        result.Value.gameObject.AddComponent<Rotate>();
        result.Value.gameObject.transform.SetParent(PreviewEmptyObject);
    }
}
