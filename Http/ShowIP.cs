using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_WSA && !UNITY_EDITOR
using Windows.Networking.Connectivity;
#endif

public class ShowIP : MonoBehaviour {

    public TextMesh text;
#if UNITY_WSA && !UNITY_EDITOR
    private string GetLocalIp()
    {
        var icp = NetworkInformation.GetInternetConnectionProfile();

        if (icp?.NetworkAdapter == null) return null;
        var hostname =
            NetworkInformation.GetHostNames()
                .SingleOrDefault(
                    hn =>
                        hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                        == icp.NetworkAdapter.NetworkAdapterId);

        // the ip address
        return hostname?.CanonicalName;
    }
#endif

    void Start () {
        text.text = new UnityEngine.Networking.NetworkManager().networkAddress;
    }
}
