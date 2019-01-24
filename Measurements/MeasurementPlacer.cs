using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasurementPlacer : MonoBehaviour {

	public HashSet<Vector3> Positions { get; private set; }
    public List<MonoMeasurement3D> Monos { get; private set; }

    private void Start()
    {
        Positions = new HashSet<Vector3>();
        Monos = new List<MonoMeasurement3D>();
    }

    public void Add(Measurement3D measurement)
    {
        Positions.Add(measurement);
        AddMono(measurement);
    }

    public bool Contains(Vector3 measurement)
    {
        return Positions.Contains(measurement);
    }
    
    public void AddMono(Measurement3D measurement)
    {
        GameObject obj = new GameObject("Measurement " + Monos.Count);
        MonoMeasurement3D mono = (obj.AddComponent<MonoMeasurement3D>());
        mono.SetSize(measurement.Transparency);
        obj.transform.parent = transform;
        Monos.Add(mono);
    }
}
