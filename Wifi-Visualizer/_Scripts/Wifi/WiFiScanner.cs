using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net.NetworkInformation;
using System;

public class WiFiScanner : MonoBehaviour
{
    private IWiFiAdapter WiFiAdapter;
    private static WiFiScanner mInstance;

    // Use this for initialization

    public static WiFiScanner Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject go = new GameObject();
                mInstance = go.AddComponent<WiFiScanner>();
                mInstance.Reset();
            }
            return mInstance;
        }
    }

    void Reset()
    {
        WiFiAdapter = new UWPWiFi();
    }

    public Measurement3D Scan(Vector3 position)
    {
            Signal signal = WiFiAdapter.GetSignal();
            return new Measurement3D(position, signal);
     
    }
}