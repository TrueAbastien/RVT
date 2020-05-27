using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Menu Panel, used to import models from Google Poly's API.
/// </summary>
[RequireComponent(typeof(CreatorMenu))]
[RequireComponent(typeof(PolygonLoader))]
public class ImportMenuPanel : AbstractMenuPanel
{
    /// <summary>
    /// Parent object to the newly imported model.
    /// </summary>
    public GameObject SceneContainer;
    /// <summary>
    /// Build panel instance (to communicate with).
    /// </summary>
    public BuilderMenuPanel buildPanel;

    /// <summary>
    /// Empty overwritten function.
    /// </summary>
    public override void handle() { }

    /// <summary>
    /// Add a specific object to the Building Scene/Panel.
    /// </summary>
    /// <param name="parent">Specific object parent object</param>
    public void AddObject(GameObject parent)
    {
        GameObject child = parent.transform.GetChild(0).gameObject;
        if (child != null)
        {
            // Duplicate first found Child (as targetted object)
            GameObject obj = Instantiate(child, Vector3.zero, Quaternion.Euler(0, 0, 0), SceneContainer.transform);
            obj.transform.localScale /= obj.transform.lossyScale.x;

            // Modify its composure
            Destroy(obj.GetComponent<Rotate>());
            obj.AddComponent<ObjectModel>().key = PolygonLoader.latestKey;
            ColliderMaker.GenerateOn(obj);
            obj.AddComponent<ObjectTransformer>().panel = buildPanel;

            // Remove the current object from parent
            if (parent.transform.childCount > 0)
                Destroy(parent.transform.GetChild(0).gameObject);
        }
    }
}
