using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoTetrahedron : MonoBehaviour
{
    private Tetrahedron tetrahedron;
    private bool initialized = false;

    private MeshFilter meshFilter;

    public void SetTetrahedron(Tetrahedron tetrahedron)
    {
        this.tetrahedron = tetrahedron;
        DestroyIfArtificial();

        if (!initialized)
        {
            Initialize();
        }

        meshFilter.mesh.vertices = tetrahedron.Vectors.ToArray();
        meshFilter.mesh.triangles = tetrahedron.Indices.ToArray();
        meshFilter.mesh.colors = tetrahedron.Colors;
    }


    private void Initialize()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        gameObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/Tetrahedron"));
        initialized = true;
    }


    private void DestroyIfArtificial()
    {
        if (tetrahedron.IsArtificial)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnDrawGizmos()
    {
        //for (int i = 0; i < tetrahedron.Triangles.Count; i += 3)
        //{
        //    Vector3 normal = tetrahedron.CalculateNormal(tetrahedron.Triangles[i], tetrahedron.Triangles[i + 1], tetrahedron.Triangles[i + 2]);
        //    Vector3 centroid = (tetrahedron.Vertices[tetrahedron.Triangles[i]] + tetrahedron.Vertices[tetrahedron.Triangles[i + 1]] + tetrahedron.Vertices[tetrahedron.Triangles[i + 2]]) / 3;
        //    Gizmos.DrawLine(centroid, centroid + 3 * normal.normalized);
        //}
    }
}
