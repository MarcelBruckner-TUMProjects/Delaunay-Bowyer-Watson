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
    private int width;
    private float size;

    public MeasurementPlacer measurementPlacer;

    private List<MonoMeasurement3D> gridPoints;
    private Vector3 lastMidpoint;


    // Use this for initialization
    void Start()
    {
        gridPoints = new List<MonoMeasurement3D>();

        lastMidpoint = Midpoint();
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
        size = Mathf.Min(dx, dy, dz) / 4f;
        width = 2 * gridRadius + 1;

        while (gridPoints.Count > width * width * width)
        {
            Destroy(gridPoints[0].gameObject);
            gridPoints.RemoveAt(0);
        }

        for (int z = -gridRadius; z <= gridRadius; z++)
        {
            for (int y = -gridRadius; y <= gridRadius; y++)
            {
                for (int x = -gridRadius; x <= gridRadius; x++)
                {
                    int i = (x + gridRadius);
                    i += (y + gridRadius) * width;
                    i += (z + gridRadius) * width * width;

                    if(i >= gridPoints.Count)
                    {
                        GameObject obj = new GameObject("Mono Measurement (" + i + ")");
                        MonoMeasurement3D mono = obj.AddComponent<MonoMeasurement3D>();
                        mono.SetSize(size);
                        mono.SetMeasurement(new Measurement3D(0, 0, 0, true), false);
                        obj.transform.parent = transform;
                        gridPoints.Add(mono);
                    }
                    

                    Vector3 v = new Vector3(lastMidpoint.x + x * dx,
                            lastMidpoint.y + y * dy,
                            lastMidpoint.z + z * dz);

                    if (measurementPlacer.Contains(v))
                    {
                        gridPoints[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        gridPoints[i].SetMeasurement(new Measurement3D(v, true), true);
                        gridPoints[i].gameObject.SetActive(true);
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