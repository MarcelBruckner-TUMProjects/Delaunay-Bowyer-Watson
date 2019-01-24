public class Signal
{
    public string SSID { get; private set; }
    public string MAC { get; private set; }
    public double Decibel { get; private set; }

    public Signal(string SSID, string MAC, double Decibel)
    {
        this.SSID = SSID;
        this.MAC = MAC;
        this.Decibel = Decibel;
    }

    public override string ToString()
    {
        return SSID + " - " + MAC + " - " + Decibel;
    }
}