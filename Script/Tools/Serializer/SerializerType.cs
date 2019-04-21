using UnityEngine;

[System.Serializable]
public class SerializableColor
{
    public float R;
    public float G;
    public float B;
    public float A;
    public SerializableColor(Color color)
    {
        R = color.r;
        G = color.g;
        B = color.b;
        A = color.a;
    }

    public SerializableColor()
    {
        R = G = B = A = 0.0f;
    }

    public Color GetColor()
    {
        return new Color(R, G, B, A);
    }
}