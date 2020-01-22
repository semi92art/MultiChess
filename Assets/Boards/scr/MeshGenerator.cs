using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public static class MeshGenerator
{
    public static void GenerateBoardItemMesh(Vector3[] points, GameObject go, Material material)
    {
        if (points == null || points.Length != 4)
            throw new System.NotImplementedException("points vector not implemented!");

        var indices = new int[6] { 0, 1, 2, 0, 2, 3 };

        var mf = go.GetComponent<MeshFilter>();
        if (mf == null)
            mf = go.AddComponent<MeshFilter>();
        var mr = go.GetComponent<MeshRenderer>();
        if (mr == null)
            mr = go.AddComponent<MeshRenderer>();
        if (mf.sharedMesh == null)
            mf.sharedMesh = new Mesh();
        mf.sharedMesh.Clear();
        mf.sharedMesh.SetVertices(points.ToList());
        mf.sharedMesh.SetIndices(indices, MeshTopology.Triangles, 0);
        mf.sharedMesh.RecalculateBounds();
        mf.sharedMesh.hideFlags = HideFlags.DontSave;
        mr.sharedMaterial = material;
    }

    
}