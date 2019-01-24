using UnityEngine;

public class ScaleSphere : MonoBehaviour {

    [Range(0, 100)]
    public float scale;

    private void OnValidate()
    {
        transform.localScale = Vector3.one * scale;
    }
}
