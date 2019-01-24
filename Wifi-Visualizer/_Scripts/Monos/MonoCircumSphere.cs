using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoCircumSphere : MonoBehaviour {
    
    public void Initialize(CircumSphere circumSphere)
    {
        transform.position = circumSphere.Center;
        transform.localScale = 2 * Vector3.one * circumSphere.Radius;
    }
}
