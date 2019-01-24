using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MarkerBehaviour : IMarkerBehaviour {
    
    public override bool IsTracked
    {
        get
        {
            var status = trackable.CurrentStatus;
            return status == TrackableBehaviour.Status.TRACKED;
        }
    }

    public override Vector3 Position
    {
        get
        {
            return gameObject.transform.position;
        }
    }
}
