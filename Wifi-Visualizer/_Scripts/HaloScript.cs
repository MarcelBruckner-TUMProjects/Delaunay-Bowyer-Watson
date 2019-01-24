using UnityEngine;

public class HaloScript : MonoBehaviour
{
    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Awake()
    {
        Start();
    }

    public void SetShader(Shader shader)
    {
        rend.material = new Material(shader);
    }

    public void SetColor(int decibel)
    {
        float value = Mathf.Clamp(decibel, -80f, -30f);
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

        Color c = new Color(r / 255f, g / 255f, 0);
        rend.material.color = c;

        rend.material.SetColor("_Color", c);
    }
}