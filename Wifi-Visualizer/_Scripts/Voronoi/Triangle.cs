using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Triangle
{
    public Measurement3D a;
    public Measurement3D b;
    public Measurement3D c;

    public Vector3 Normal
    {
        get
        {
            return Vector3.Normalize(Vector3.Cross(b.Position - a, c.Position - a));
        }
    }

    public Triangle(Measurement3D a, Measurement3D b, Measurement3D c)
    {
        if (a.Equals(b) || b.Equals(c) || c.Equals(a))
        {
            throw new ArgumentException("Two points are equal!");
        }
        this.a = a;
        this.b = b;
        this.c = c;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Triangle))
        {
            return false;
        }
        Triangle other = (Triangle)obj;
        return
            (other.a.Equals(a) && other.b.Equals(b) && other.c.Equals(c)) ||
            (other.a.Equals(a) && other.b.Equals(c) && other.c.Equals(b)) ||
            (other.a.Equals(b) && other.b.Equals(a) && other.c.Equals(c)) ||
            (other.a.Equals(b) && other.b.Equals(c) && other.c.Equals(a)) ||
            (other.a.Equals(c) && other.b.Equals(a) && other.c.Equals(b)) ||
            (other.a.Equals(c) && other.b.Equals(b) && other.c.Equals(a));
    }

    public bool InSamePlane(Measurement3D other)
    {
        return Vector3.Dot(Normal, other.Position - a) == 0;
    }

    public override int GetHashCode()
    {
        var hashCode = 1474027755;
        hashCode = hashCode * a.GetHashCode();
        hashCode = hashCode * b.GetHashCode();
        hashCode = hashCode * c.GetHashCode();
        return hashCode;
    }
}
