using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelaunayTriangulatorTest : MonoBehaviour
{
    public TextMesh text;
    public Shader tetrahedron;
    public Shader measurement;
    float delay = 0.5f;
    float size = 2;
    float lastUpdate;
    int i = 0;
    void Start()
    {
        //new Tetrahedron(
        //    new Measurement3D(0, 0, 0),
        //    new Measurement3D(0, 0, 1),
        //    new Measurement3D(0, 1, 0),
        //    new Measurement3D(1, 0, 0)
        //    );
        
        //new Tetrahedron(
        //    new Measurement3D(1, 0, 1),
        //    new Measurement3D(0, 0, 1),
        //    new Measurement3D(0, 1, 0),
        //    new Measurement3D(1, 0, 0)
        //    );

        //GameObject gameObject = new GameObject("test");
        //gameObject.AddComponent<Meshre>

    }

    private void Update()
    {
        if (i < 100 && Time.time - lastUpdate > 0.5)
        {
            lastUpdate = Time.time;
            DelaunayTriangulator.Instance.Add(new Measurement3D(Random.Range(-size / 2, size / 2), Random.Range(-size / 2, size / 2), Random.Range(1, size + 1), "", "", Random.Range(-80, -30), false));
            i++;
            text.text = i + "";
        }
    }

    private IEnumerator IterativeRandomTest()
    {
        for (int i = 0; i < 100; i++)
        {
            DelaunayTriangulator.Instance.Add(new Measurement3D(Random.Range(-size / 2, size / 2), Random.Range(-size / 2, size / 2), Random.Range(1, size + 1), "", "", Random.Range(-80, -30), false));
            yield return new WaitForSeconds(delay);
        }
    }

    private void ManyRandomTest()
    {
        List<Measurement3D> measurements = new List<Measurement3D>();
        for (int i = 0; i < 100; i++)
        {
            measurements.Add(new Measurement3D(Random.Range(-size / 2, size / 2), Random.Range(-size / 2, size / 2), Random.Range(1, size + 1), "", "", Random.Range(-80, -30), false));
        }
        DelaunayTriangulator.Instance.Generate(measurements);
    }

    private IEnumerator CubeTest()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    DelaunayTriangulator.Instance.Add(new Measurement3D(i * 10, j * 10, k * 10, "", "", Random.Range(-80, -30), false));
                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }
}
