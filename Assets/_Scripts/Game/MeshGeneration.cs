using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGeneration : MonoBehaviour {

    public int NumPoints = 20;
    public int Radius = 5;

    void Start () {

        var o = new GameObject();
        o.name = "Circle Mesh";
        
        var renderer = o.AddComponent<MeshRenderer>();
        var filter = o.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();
        filter.mesh = mesh;

        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var normals = new List<Vector3>();

        float angleStep = 360.0f / NumPoints;
        Quaternion quaternion = Quaternion.Euler(0.0f, 0.0f, angleStep);

        vertices.Add(new Vector3(0.0f, 0.0f, 0.0f));
        vertices.Add(new Vector3(0.0f, Radius, 0.0f));
        vertices.Add(quaternion * vertices[1]);

        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);

        for (int i = 0; i < NumPoints - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(vertices.Count - 1);
            triangles.Add(vertices.Count);

            vertices.Add(quaternion * vertices[vertices.Count - 1]);
        }

        for (int i = 0; i < NumPoints + 2; i++)
        {
            normals.Add(-Vector3.forward);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
    }
}
