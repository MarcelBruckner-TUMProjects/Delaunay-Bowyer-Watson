using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircumSphereTest : MonoBehaviour {

    public MonoTetrahedron tetraPrefab;
    public MonoCircumSphere circumPrefab;

	// Use this for initialization
	void Start () {
        Tetrahedron tetrahedron = new Tetrahedron(
            new Measurement3D(4,-2,6),
            new Measurement3D(-5, 0, 2),
            new Measurement3D(7,1,-3),
            new Measurement3D(0,5,1));

        MonoTetrahedron monoTetrahedron = Instantiate(tetraPrefab);
        MonoCircumSphere monoCircumSphere = Instantiate(circumPrefab);

        monoTetrahedron.SetTetrahedron(tetrahedron);
        monoCircumSphere.Initialize(tetrahedron.CircumSphere);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
