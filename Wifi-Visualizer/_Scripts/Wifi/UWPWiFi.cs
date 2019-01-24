using System;
using System.Collections.Generic;
using System.Linq;

#if !UNITY_EDITOR
using System.Threading.Tasks;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;
#endif

public class UWPWiFi : IWiFiAdapter
{
    private bool IsReady { get; set; }
    private string Ssid { get; set; }
    private Signal Signal { get; set; }
    private Random random;

    public UWPWiFi()
    {
        random = new Random();
        IsReady = true;
        Signal = new Signal("", "", 1);
        Ssid = string.Empty;
    }

#if !UNITY_EDITOR

    public Signal GetSignal()
    {
        GetSsid();
        Scan();
        return Signal;
    }

    private void GetSsid(){
        ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
        Ssid = profile.ProfileName;
    }

    private async void Scan()
    {
        if (IsReady)
        {
            IsReady = false;
            Signal signal = new Signal("","",1);
            var result = await WiFiAdapter.FindAllAdaptersAsync();
            if (result.Count >= 1)
            {
                var firstAdapter = result[0];
                await firstAdapter.ScanAsync();
                if (!string.IsNullOrEmpty(Ssid))
                {
                    signal = GetNetworkSignal(firstAdapter.NetworkReport, Ssid);
                }
            }
            IsReady = true;
            Signal = signal;
        }
    }

    private Signal GetNetworkSignal(WiFiNetworkReport report, string ssid)
    {
        var network = report.AvailableNetworks.Where(x => x.Ssid.ToLower() == ssid.ToLower()).FirstOrDefault();
        return new Signal(ssid, network.Bssid, network.NetworkRssiInDecibelMilliwatts);
    }
#else
    public Signal GetSignal()
    {
        return new Signal("Test", "Test", random.Next(-80, -30));
    }
#endif
}
