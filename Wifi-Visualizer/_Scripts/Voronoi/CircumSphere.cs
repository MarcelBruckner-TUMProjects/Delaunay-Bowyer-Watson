using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircumSphere {
    public Vector3 Center { get; private set; }
    public float Radius { get; private set; }
    private float a;
    private float dx;
    private float dy;
    private float dz;
    private float c;

    private float[,] matrix_buffer = new float[4, 4];

        public CircumSphere(params Vector3[] vectors)
    {
        if(vectors.Length != 4)
        {
            throw new ArgumentException("For Circumsphere calculation one needs 4 vectors!");
        }
        a = A(vectors);
        dx = Dx(vectors);
        dy = Dy(vectors);
        dz = Dz(vectors);
        c = C(vectors);
        CalculateCenter(vectors);
        CalculateRadius(vectors);
    }

    public CircumSphere(List<Vector3> vectors) : this(vectors.ToArray()) { }

    public CircumSphere(List<Measurement3D> vectors) : this(vectors[0], vectors[1], vectors[2], vectors[3]) { }

    private float Determinant()
    {
        float buffer = 0;

        buffer += matrix_buffer[0,0] * Sarrus(1, 2, 3);
        buffer -= matrix_buffer[1,0] * Sarrus(0, 2, 3);
        buffer += matrix_buffer[2,0] * Sarrus(0, 1, 3);
        buffer -= matrix_buffer[3,0] * Sarrus(0, 1, 2);

        return buffer;
    }

    private float Sarrus(int r1, int r2, int r3)
    {
        float sum = matrix_buffer[r1, 1] * matrix_buffer[r2,2] * matrix_buffer[r3,3];
        sum += matrix_buffer[r1, 2] * matrix_buffer[r2, 3] * matrix_buffer[r3, 1];
        sum += matrix_buffer[r1, 3] * matrix_buffer[r2, 1] * matrix_buffer[r3, 2];
        sum -= matrix_buffer[r3, 1] * matrix_buffer[r2, 2] * matrix_buffer[r1, 3];
        sum -= matrix_buffer[r3, 2] * matrix_buffer[r2, 3] * matrix_buffer[r1, 1];
        sum -= matrix_buffer[r3, 3] * matrix_buffer[r2, 1] * matrix_buffer[r1, 2];
        return sum;
    }

    private float A(params Vector3[] vectors)
    {
        for(int i = 0; i < 4; i++)
        {
            matrix_buffer[i, 0] = vectors[i].x;
            matrix_buffer[i, 1] = vectors[i].y;
            matrix_buffer[i, 2] = vectors[i].z;
            matrix_buffer[i, 3] = 1;
        }
        return Determinant();
    }

    private float Dx(params Vector3[] vectors)
    {
        for (int i = 0; i < 4; i++)
        {
            float sqrMagnitude = vectors[i].x * vectors[i].x + vectors[i].y * vectors[i].y + vectors[i].z * vectors[i].z;
            matrix_buffer[i, 0] = sqrMagnitude;
            matrix_buffer[i, 1] = vectors[i].y;
            matrix_buffer[i, 2] = vectors[i].z;
            matrix_buffer[i, 3] = 1;
        }
        return Determinant();
    }

    private float Dy(params Vector3[] vectors)
    {
        for (int i = 0; i < 4; i++)
        {
            float sqrMagnitude = vectors[i].x * vectors[i].x + vectors[i].y * vectors[i].y + vectors[i].z * vectors[i].z;
            matrix_buffer[i, 0] = sqrMagnitude;
            matrix_buffer[i, 1] = vectors[i].x;
            matrix_buffer[i, 2] = vectors[i].z;
            matrix_buffer[i, 3] = 1;
        }
        return -Determinant();
    }

    private float Dz(params Vector3[] vectors)
    {
        for (int i = 0; i < 4; i++)
        {
            float sqrMagnitude = vectors[i].x * vectors[i].x + vectors[i].y * vectors[i].y + vectors[i].z * vectors[i].z;
            matrix_buffer[i, 0] = sqrMagnitude;
            matrix_buffer[i, 1] = vectors[i].x;
            matrix_buffer[i, 2] = vectors[i].y;
            matrix_buffer[i, 3] = 1;
        }
        return Determinant();
    }

    private float C(params Vector3[] vectors)
    {
        for (int i = 0; i < 4; i++)
        {
            float sqrMagnitude = vectors[i].x * vectors[i].x + vectors[i].y * vectors[i].y + vectors[i].z * vectors[i].z;
            matrix_buffer[i, 0] = sqrMagnitude;
            matrix_buffer[i, 1] = vectors[i].x;
            matrix_buffer[i, 2] = vectors[i].y;
            matrix_buffer[i, 3] = vectors[i].z;
        }
        return Determinant();
    }

    public void CalculateCenter(params Vector3[] vectors)
    {
        Center = new Vector3(Dx(vectors) / (2 * a), Dy(vectors) / (2 * a), Dz(vectors) / (2 * a));
    }

    public void CalculateRadius(params Vector3[] vectors)
    {
        Radius = Mathf.Sqrt((dx * dx) + (dy * dy) + (dz * dz) - (4 * a * c)) / (2 * Mathf.Abs(a));
    }

    public bool Includes(Vector3 other)
    {
        return Vector3.Distance(Center, other) < Radius ;
    }
}