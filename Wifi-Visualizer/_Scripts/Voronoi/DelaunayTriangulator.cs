using System;
using System.Collections.Generic;
using UnityEngine;

public class DelaunayTriangulator : MonoBehaviour
{
    private List<MonoTetrahedron> tetrahedrons;
    private List<MonoMeasurement3D> measurements;

    private GameObject allMeshes;

    private GameObject measurementsContainer;
    private GameObject tetrahedronsContainer;

    public IDelaunayTriangulation Triangulation { get; private set; }

    static DelaunayTriangulator mInstance;

    Queue<Measurement3D> toAddQueue;

    public static DelaunayTriangulator Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject go = new GameObject();
                mInstance = go.AddComponent<DelaunayTriangulator>();
                mInstance.Reset();
            }
            return mInstance;
        }
    }

    public void Reset()
    {
        toAddQueue = new Queue<Measurement3D>();
        Triangulation = new DelaunayTriangulation();
        tetrahedrons = new List<MonoTetrahedron>();
        measurements = new List<MonoMeasurement3D>();
        measurementsContainer = new GameObject("Measurements");
        tetrahedronsContainer = new GameObject("Tetrahedrons");
        allMeshes = new GameObject("All Meshed");
        measurementsContainer.transform.parent = transform;
        tetrahedronsContainer.transform.parent = transform;
        allMeshes.transform.parent = transform;
        allMeshes.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/Tetrahedron"));
        allMeshes.AddComponent<MeshFilter>();
    }

    private void Update()
    {
        if(mInstance == null)
        {
            return;
        }

        if (!Triangulation.IsBusy)
        {
            if (toAddQueue.Count > 0)
            {
                Triangulation.Add(toAddQueue.Dequeue());
            }
        }

        if (Triangulation.IsUpdated)
        {
            Render();
            Triangulation.IsUpdated = false;
        }
    }

    public void Add(Measurement3D measurement)
    {
        toAddQueue.Enqueue(measurement);
        //Render();
    }

    public void AddAll(List<Measurement3D> measurements)
    {
        measurements.ForEach(m => toAddQueue.Enqueue(m));
    }

    public void Generate(List<Measurement3D> measurements)
    {
        Triangulation.Generate(measurements);
    }

    private void Render()
    {
        RenderTetrahedrons();
        RenderMeasurements();
    }

    private void RenderMeasurements()
    {
        float size = Triangulation.AverageDistance * 0.1f;

        for (int i = 0; i < Triangulation.Measurements.Count; i++)
        {
            while (i >= measurements.Count)
            {
                GameObject obj = new GameObject("Measurement " + i);
                MonoMeasurement3D mono = (obj.AddComponent<MonoMeasurement3D>());
                measurements.Add(mono);
                mono.SetSize(size);
                obj.transform.parent = measurementsContainer.transform;
            }
            measurements[i].SetMeasurement(Triangulation.Measurements[i], false);
            measurements[i].gameObject.SetActive(true);
        }
    }

    private void RenderTetrahedrons()
    {
        //RenderAllByItself();
        //RenderAllInOne();
    }

    private void RenderAllInOne()
    {
        IEnumerator<Tetrahedron> toRender = Triangulation.Tetrahedrons.GetEnumerator();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();

        int i = 0;
        while (toRender.MoveNext())
        {
            Tetrahedron tetrahedron = toRender.Current;
            vertices.AddRange(tetrahedron.Vectors);
            colors.AddRange(tetrahedron.Colors);

            foreach (int x in tetrahedron.Indices)
            {
                triangles.Add(x + 4 * i);
            }

            i++;
        }

        MeshFilter filter = allMeshes.GetComponent<MeshFilter>();
        filter.mesh.vertices = vertices.ToArray();
        filter.mesh.triangles = triangles.ToArray();
        filter.mesh.colors = colors.ToArray();
    }

    private void RenderAllByItself()
    {
        IEnumerator<Tetrahedron> toRender = Triangulation.Tetrahedrons.GetEnumerator();
        int i = 0;
        while (toRender.MoveNext())
        {
            if (i >= tetrahedrons.Count)
            {
                GameObject obj = new GameObject("Tetrahedron " + i);
                tetrahedrons.Add(obj.AddComponent<MonoTetrahedron>());
                obj.SetActive(true);
                obj.transform.parent = tetrahedronsContainer.transform;
            }
            tetrahedrons[i].SetTetrahedron(toRender.Current);
            i++;
        }
    }
}