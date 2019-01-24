using System.Collections.Generic;
using UnityEngine;

public class Tetrahedron
{
    public List<Measurement3D> Measurements { get; private set; }
    public List<int> Indices { get; private set; }
    public List<Triangle> Triangles { get; private set; }
    public CircumSphere CircumSphere { get; private set; }
    public bool IsArtificial { get; private set; }

    public Tetrahedron(Measurement3D a, Measurement3D b, Measurement3D c, Measurement3D d)
    {
        Measurements = new List<Measurement3D>()
        {
            a,b,c,d
        };
        CalculateIndices();
        CalculateArtificial();
        CircumSphere = new CircumSphere(Measurements);
    }

    public Tetrahedron(params Measurement3D[] measurements) : this(measurements[0], measurements[1], measurements[2], measurements[3]) { }

    public List<Vector3> Vectors
    {
        get
        {
            List<Vector3> vectors = new List<Vector3>();
            foreach(Measurement3D measurement in Measurements)
            {
                vectors.Add(measurement);
            }
            return vectors;
        }
    }

    private void CalculateIndices()
    {
        Indices = new List<int>();
        Triangles = new List<Triangle>();
        CalculateIndices(0, 1, 2, 3);
        CalculateIndices(0, 1, 3, 2);
        CalculateIndices(0, 2, 3, 1);
        CalculateIndices(3, 1, 2, 0);
    }
    private void CalculateIndices(int p1, int p2, int p3, int vv)
    {
        Triangle triangle= new Triangle(Measurements[p1], Measurements[p2], Measurements[p3]);
        Triangles.Add(triangle);
        
        Vector3 v = Measurements[vv];

        float dotp = Vector3.Dot(triangle.Normal, v - Measurements[p1]);
        Indices.Add(p1);
        if (dotp < 0)
        {
            Indices.Add(p2);
            Indices.Add(p3);
        }
        else
        {
            Indices.Add(p3);
            Indices.Add(p2);
        }
    }

    public static float Volume(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        return Vector3.Dot(Vector3.Cross(b - a, c - a), d - a) / 6f;
    }
    public static float Volume(Measurement3D a, Measurement3D b, Measurement3D c, Vector3 d)
    {
        return Volume(a.Position, b.Position, c.Position, d);
    }
    public Color BarycentricInterpolation(Vector3 measurement)
    {
        Color color = new Color(0,0,0,0);

        color += Multiply(Measurements[0].Color, Volume(Measurements[1], Measurements[2], Measurements[3], measurement));
        color += Multiply(Measurements[1].Color, Volume(Measurements[2], Measurements[3], Measurements[0], measurement));
        color += Multiply(Measurements[2].Color, Volume(Measurements[3], Measurements[0], Measurements[1], measurement));
        color += Multiply(Measurements[3].Color, Volume(Measurements[0], Measurements[1], Measurements[2], measurement));

        return color;
    }
    private static Color Multiply(Color color, float value)
    {
        return new Color(
        color.a * value,
        color.r * value,
        color.g * value,
        color.b * value);
    }
    public bool Includes(Measurement3D measurement)
    {
        return CircumSphere.Includes(measurement.Position);
    }

    public void CalculateArtificial()
    {
        IsArtificial = false;
        foreach(Measurement3D measurement in Measurements)
        {
            if (measurement.IsArtificial)
            {
                IsArtificial = true;
                return;
            }
        }
    }

    public Color[] Colors
    {
        get
        {
            List<Color> colors = new List<Color>();
            foreach (Measurement3D measurement in Measurements)
            {
                colors.Add(measurement.Color);
            }
            return colors.ToArray();
        }
    }
}