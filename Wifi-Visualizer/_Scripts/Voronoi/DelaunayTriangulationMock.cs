using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DelaunayTriangulationMock : IDelaunayTriangulation
{
    public DelaunayTriangulationMock() : base() { }

    public override void Add(Measurement3D measurement)
    {
    }

    public override void AddAll(List<Measurement3D> measurement)
    {
    }

    public override void Generate(List<Measurement3D> measurements)
    {
        Random.InitState(0);
        Measurements = new List<Measurement3D>(); ;
        Triangulation = new List<Tetrahedron>();

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    Add(new Measurement3D(i * 10, j * 10, k * 10));
                }
            }
        }

        Triangulation.Add(new Tetrahedron(Measurements[0], Measurements[1], Measurements[3], Measurements[7]));
        Triangulation.Add(new Tetrahedron(Measurements[0], Measurements[2], Measurements[3], Measurements[7]));
        Triangulation.Add(new Tetrahedron(Measurements[0], Measurements[1], Measurements[5], Measurements[7]));
        Triangulation.Add(new Tetrahedron(Measurements[0], Measurements[4], Measurements[5], Measurements[7]));
        Triangulation.Add(new Tetrahedron(Measurements[0], Measurements[2], Measurements[6], Measurements[7]));
        Triangulation.Add(new Tetrahedron(Measurements[0], Measurements[4], Measurements[6], Measurements[7]));
    }
}