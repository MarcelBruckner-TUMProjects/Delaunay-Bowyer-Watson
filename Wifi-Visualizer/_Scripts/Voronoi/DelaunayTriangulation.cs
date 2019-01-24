using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

#if !UNITY_EDITOR && UNITY_METRO
using System.Threading;
using System.Threading.Tasks;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif


public class DelaunayTriangulation : IDelaunayTriangulation
{
    public DelaunayTriangulation() : base() { }

    public override void Add(Measurement3D measurement)
    {
        Add_Coroutine(measurement);
    }

    private void Add_Coroutine(Measurement3D measurement)
    {
        IsBusy = true;
        Measurements.Add(measurement);
        Positions.Add(measurement);

        //      badTriangles:= empty set
        List<Tetrahedron> badTetrahedrons = GetBadTetrahedrons(measurement);

        //   polygon := empty set
        List<Triangle> polygon = TriangleTest(badTetrahedrons);

        UpdateTriangulation(measurement, badTetrahedrons, polygon);
        IsBusy = false;
        IsUpdated = true;
    }

    private void UpdateTriangulation(Measurement3D measurement, List<Tetrahedron> badTetrahedrons, List<Triangle> polygon)
    {
        //   for each triangle in badTriangles do // remove them from the data structure
        foreach (Tetrahedron tetrahedron in badTetrahedrons)
        {
            //          remove triangle from triangulation
            Triangulation.Remove(tetrahedron);
        }
        //   for each edge in polygon do // re-triangulate the polygonal hole
        foreach (Triangle triangle in polygon)
        {
            //      newTri:= form a triangle from edge to point
            //     add newTri to triangulation
            if (triangle.InSamePlane(measurement))
            {
                Debug.Log("In same plane");
                continue;
            }
            Tetrahedron tetrahedron = new Tetrahedron(
                    measurement,
                    triangle.a,
                    triangle.b,
                    triangle.c);
            Triangulation.Add(tetrahedron);
        }
    }

    private List<Triangle> TriangleTest(List<Tetrahedron> badTetrahedrons)
    {
        List<Triangle> polygon = new List<Triangle>();

        //   for each triangle in badTriangles do // find the boundary of the polygonal hole
        foreach (Tetrahedron tetrahedron in badTetrahedrons)
        {
            //          for each edge in triangle do
            foreach (Triangle triangle in tetrahedron.Triangles)
            {
                //                  if edge is not shared by any other triangles in badTriangles
                if (!EdgeInBadTriangles(badTetrahedrons.Where(t => !t.Equals(tetrahedron)), triangle))
                {
                    //                     add edge to polygon
                    polygon.Add(triangle);
                }
            }
        }
        return polygon;
    }

    private List<Tetrahedron> GetBadTetrahedrons(Measurement3D measurement)
    {
        List<Tetrahedron> badTetrahedrons = new List<Tetrahedron>();
        //   for each triangle in triangulation do // first find all the triangles that are no longer valid due to the insertion
        foreach (Tetrahedron tetrahedron in Triangulation)
        {
            //          if point is inside circumcircle of triangle
            if (tetrahedron.Includes(measurement))
            {
                //         add triangle to badTriangles
                badTetrahedrons.Add(tetrahedron);
            }
        }
        return badTetrahedrons;
    }

    public override void Generate(List<Measurement3D> measurements)
    {
        //triangulation:= empty triangle mesh data structure
        //add super-triangle to triangulation // must be large enough to completely contain all the points in pointList
        Init();
        //for each point in pointList do // add all the points one at a time to the triangulation
        AddAll(measurements);
    }


    public bool EdgeInBadTriangles(IEnumerable<Tetrahedron> tetrahedrons, Triangle triangle)
    {
        //                  if edge is not shared by any other triangles in badTriangles
        foreach (Tetrahedron otherTetrahedron in tetrahedrons)
        {
                if (otherTetrahedron.Triangles.Contains(triangle))
                {
                    return true;
                }
        }
        return false;
    }

    public override void AddAll(List<Measurement3D> measurements)
    {
        AddAll_Coroutine(measurements);
    }

    private void AddAll_Coroutine(List<Measurement3D> measurements)
    {
        IsBusy = true;
        foreach (Measurement3D measurement in measurements)
        {
            Add(measurement);
            IsUpdated = false;
        }
        IsBusy = false;
    }
}