using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StreamSocketHttpServer;
using System.Text;
using Vuforia;

[UnityHttpServer]
public class CaptureRoute : MonoBehaviour {

    public TextMesh text;
    public IMarkerBehaviour marker;

    [UnityHttpRoute("/capture")]
    public void RouteIndex(HttpRequest request, HttpResponse response) {
        string s = "";
        foreach(KeyValuePair<string, string> pair in request.Args)
        {
            s += pair.Key + " - " + pair.Value + "\n";
        }

        response.BodyData = Encoding.UTF8.GetBytes(s);
        text.text = s;

        if (marker.IsTracked)
        {
            Measurement3D measurement = new Measurement3D(marker.Position, request.Args["ssid"], request.Args["mac"], int.Parse(request.Args["db"]));
        }
    }
}
