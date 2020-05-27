using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Save the current children in Scene Transcriver instance.
/// </summary>
[RequireComponent(typeof(SceneTranscriver))]
public class SceneSaver : MonoBehaviour
{
    /// <summary>
    /// Player object representation.
    /// </summary>
    public GameObject player;
    /// <summary>
    /// Parent for every model object.
    /// </summary>
    public GameObject parent;

    /// <summary>
    /// List of all models as Model Data.
    /// </summary>
    private List<SceneData.ModelData> models;

    /// <summary>
    /// Save every children of 'parent' object as models.
    /// </summary>
    public void SaveChildren()
    {
        models = new List<SceneData.ModelData>();
        foreach (ObjectModel model in parent.GetComponentsInChildren<ObjectModel>())
            models.Add(model.get());
        GetComponent<SceneTranscriver>().data = new SceneData(models,
            player.transform.position, player.transform.rotation.eulerAngles, player.transform.lossyScale);
    }
}
