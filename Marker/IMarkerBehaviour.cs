using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public abstract class IMarkerBehaviour : MonoBehaviour {

    protected TrackableBehaviour trackable;
    public TextMesh debug;

    // Use this for initialization
    void Start()
    {
        trackable = GetComponent<TrackableBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        debug.text = IsTracked + "";
    }

    public abstract bool IsTracked { get; }
    public abstract Vector3 Position { get; }
}
