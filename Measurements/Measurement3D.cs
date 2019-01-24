using UnityEngine;

public class Measurement3D
{
    public string Id { get; private set; }
    public bool IsArtificial { get; private set; }

    public Vector3 Position { get; private set; }
    public string SSID { get; private set; }
    public string MAC { get; private set; }
    public double Decibel { get; private set; }

    public float[] PositionArray { get { return new float[] { Position.x, Position.y, Position.z }; } }

    public Color Color
    {
        get
        {
            if (IsArtificial)
            {
                return Color.blue;
            }

            float value = Mathf.Clamp((float)Decibel, -80f, -30f);
            value += 30;
            value *= -1;

            float r = 0f;
            float g = 255f;

            if (value <= 25)
            {
                r = value / 25f * 255f;
            }
            else
            {
                r = 255f;
                g = 255f - ((value / 25f) * 255f);
            }

            return new Color(r / 255f, g / 255f, 0, Transparency);
        }
    }

    public float Falloff
    {
        get
        {
            //if (IsArtificial)
            //{
                return 2;
            //}
            //return (float)((Decibel + 80) / 50.0f) * 6 + 2;
        }
    }

    public float Transparency
    {
        get
        {
            if (IsArtificial)
            {
                return 1.5f;
            }
            return (float)((Decibel + 30.0f) / -100.0f) * 2;
        }
    }

    public Measurement3D(Vector3 position, string ssid = "", string mac = "", double decibel = -30, bool isArtificial = false)
    {
        Position = position;
        SSID = ssid;
        MAC = mac;
        Decibel = decibel;
        IsArtificial = isArtificial;
        Id = Position.ToString() + " - " + SSID + " - " + MAC + " - " + Decibel + " - " + isArtificial;
    }
    public Measurement3D(float x, float y, float z, string ssid = "", string mac = "", double decibel = -30, bool isArtificial = false) : this(new Vector3(x, y, z), ssid, mac, decibel, isArtificial) { }
    public Measurement3D(float x, float y, float z, bool isArtificial = false) : this(new Vector3(x, y, z), "", "", 0, isArtificial) { }
    public Measurement3D(float x, float y, float z) : this(new Vector3(x, y, z), "", "", 0, false) { }
    public Measurement3D(Vector3 position, Signal signal) : this(new Vector3(position.x, position.y, position.z), signal.SSID, signal.MAC, signal.Decibel, false) { }
    public Measurement3D(Vector3 position, bool isArtificial = false) : this(position, "", "", 1, isArtificial) { }

    public static implicit operator Vector3(Measurement3D measurement)
    {
        return measurement.Position;
    }

    public static implicit operator float[] (Measurement3D measurement)
    {
        return measurement.PositionArray;
    }

    public override string ToString()
    {
        return Id;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return ((Measurement3D)obj).Position.Equals(Position);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }
}