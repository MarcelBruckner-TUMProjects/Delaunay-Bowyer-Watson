using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public float dx;
    public float dy;
    public float dz;

    [Range(1,5)]
    public int gridRadius;

    private List<MonoMeasurement3D> measurements;
    private Vector3 lastMidpoint;


    // Use this for initialization
    void Start()
    {
        measurements = new List<MonoMeasurement3D>();
        lastMidpoint = Midpoint();
        float size = Mathf.Min(dx, dy, dz) / 4f;

        for (int i = 0; i < 125; i++)
        {
            GameObject obj = new GameObject("Mono Measurement (" + i + ")");
            MonoMeasurement3D mono = obj.AddComponent<MonoMeasurement3D>();
            mono.SetSize(size);
            mono.SetMeasurement(new Measurement3D(0, 0, 0, true), false);
            obj.transform.parent = transform;
            measurements.Add(mono);
        }

        SetPoints();
    }

    private void Update()
    {
        if (lastMidpoint == Midpoint())
        {
            return;
        }

        SetPoints();
    }

    private void SetPoints()
    {
        lastMidpoint = Midpoint();

        for (int x = -gridRadius; x <= gridRadius; x++)
        {
            for (int y = -gridRadius; y <= gridRadius; y++)
            {
                for (int z = -gridRadius; z <= gridRadius; z++)
                {
                    int i = (x + gridRadius) + (y + gridRadius) * (2 * gridRadius + 1) + (z + gridRadius) * (2 * gridRadius + 1) * (2 * gridRadius + 1);
                    Vector3 v = new Vector3(lastMidpoint.x + x * dx,
                            lastMidpoint.y + y * dy,
                            lastMidpoint.z + z * dz);

                    if (DelaunayTriangulator.Instance.Triangulation.Contains(v))
                    {
                        measurements[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        measurements[i].SetMeasurement(new Measurement3D(v, true), true);
                        measurements[i].gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    private Vector3 Midpoint()
    {
        Vector3 midpoint = Camera.main.transform.position;
        float x = Nearest(midpoint.x, dx);
        float y = Nearest(midpoint.y, dy);
        float z = Nearest(midpoint.z, dz);
        return new Vector3(x, y, z);
    }

    private float Nearest(float current, float delta)
    {
        if (current < 0)
        {
            return NearestNeg(current, delta);
        }
        else if (current > 0)
        {
            return NearestPos(current, delta);
        }
        return 0;
    }

    private float NearestPos(float current, float delta)
    {
        float actual = current;

        while (actual > delta)
        {
            actual -= delta;
        }

        float value;

        if (actual > delta / 2)
        {
            value = current - actual + delta;
        }
        else
        {
            value = current - actual;
        }
        return value;
    }

    private float NearestNeg(float current, float delta)
    {
        float actual = Mathf.Abs(current);

        while (actual > delta)
        {
            actual -= delta;
        }

        float value;
        if (actual > delta / 2)
        {
            value = current - (delta - actual);
        }
        else
        {
            value = current + actual;
        }
        return value;
    }
}