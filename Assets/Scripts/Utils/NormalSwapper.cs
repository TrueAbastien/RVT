using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class NormalSwapper : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;

    private int[] indexes;
    private Vector3[] vertices, normals;
    private Vector2[] uvs;

    // On Awake Function
    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        indexes = mesh.triangles;
        vertices = mesh.vertices;
        normals = mesh.normals;
        uvs = mesh.uv;

        int size = normals.Length;
        for (int ii = 0; ii < size; ++ii)
            normals[ii] = -normals[ii];
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = indexes;
        meshFilter.mesh = mesh;
    }
}
