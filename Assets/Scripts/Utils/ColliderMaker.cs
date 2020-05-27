using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generate a Mesh Collider, composite of every mesh children, on the current instance object.
/// </summary>
public class ColliderMaker : MonoBehaviour
{
    /// <summary>
    /// Start Function, generate mesh collider.
    /// </summary>
    void Start()
    {
        ColliderGeneration();
    }

    /// <summary>
    /// Create a combined mesh and store it in a MeshFilter.
    /// Add and set a 'sharedMesh' on a MeshCollider.
    /// Destroy the script instance afterwards.
    /// </summary>
    public void ColliderGeneration()
    {
        // Init Basic Components
        if (GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();
        if (GetComponent<MeshCollider>() == null)
            gameObject.AddComponent<MeshCollider>();

        // Init Variables
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        // Construct Mesh Combine
        for (int ii = 0; ii < meshFilters.Length; ++ii)
        {
            combine[ii].mesh = meshFilters[ii].sharedMesh;
            combine[ii].transform = meshFilters[ii].transform.localToWorldMatrix;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

        // Setup Mesh Collider
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;

        // Destroy current Component
        Destroy(this);
    }

    /// <summary>
    /// Allow for user to quickly add the Script and generate the collider.
    /// </summary>
    /// <param name="obj">Object on which to add the collider</param>
    /// <param name="instant">Should the collider be generated right now or at next frame (on Start)</param>
    public static void GenerateOn(GameObject obj, bool instant = false)
    {
        if (instant)
            obj.AddComponent<ColliderMaker>().ColliderGeneration();
        else obj.AddComponent<ColliderMaker>();
    }
}
