using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Synchronizer : NetworkBehaviour {

    float lastUpdate = 0;

    private void Update()
    {
        if (!isServer && Time.time - lastUpdate > 1)
        {
            CmdRetrieve(Random.Range(0, 100) + "", Random.Range(0, 100) + "", Random.Range(0, 100));
            lastUpdate = Time.time;
        }
    }

    [Command]
    public void CmdRetrieve(string mac, string ssid, int decibel)
    {
        Debug.Log("received: " + mac + "; " + ssid + "; " + decibel);
    }
}
