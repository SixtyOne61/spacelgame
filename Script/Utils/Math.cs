using UnityEngine;

public class Math
{
    private static object angle;

    public static float IntersectionCoef(Vector3 X, Vector3 forward, Vector3 P)
    {
        if (forward == Vector3.zero)
        {
            Debug.LogError("[IntersectionCoef] NaN, Div 0.");
            return 0.0f;
        }
        float num = 0.0f;
        float den = 0.0f;

        for (int i = 0; i < 3; ++i)
        {
            num += forward[i] * (P[i] - X[i]);
            den += Mathf.Pow(forward[i], 2.0f);
        }

        return num / den;
    }

    public static Vector3 ProjectionPoint(Vector3 X, Vector3 forward, Vector3 P)
    {
        return X + Math.IntersectionCoef(X, forward, P) * forward;
    }

    public static Vector3 MultipleVector(Vector3 X, Vector3 Y)
    {
        return new Vector3(X.x * Y.x, X.y * Y.y, X.z * Y.z);
    }

    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 centre, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - centre) + centre;
    }

    public static Vector3 RoundToInt(Vector3 source)
    {
        return new Vector3(Mathf.RoundToInt(source.x), Mathf.RoundToInt(source.y), Mathf.RoundToInt(source.z));
    }

    public static float PerlinNoise3D(float x, float y, float z, float frequency, int octaves)
    {
        return Noise.Noise.GetOctaveNoise(x * frequency, y * frequency, z * frequency, octaves);
    }

    public static Vector3 Abs(Vector3 vec)
    {
        return new Vector3(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z));
    }

    public static void Born(ref Vector3 source, Vector3 inf)
    {
        for (int i = 0; i < 3; ++i)
        {
            if (source[i] < inf[i])
            {
                source[i] = inf[i];
            }
        }
    }

    public static void Born(ref Vector3 source, Vector3 inf, Vector3 sup)
    {
        for (int i = 0; i < 3; ++i)
        {
            if (source[i] < inf[i])
            {
                source[i] = inf[i];
            }
            else if (source[i] > sup[i])
            {
                source[i] = sup[i];
            }
        }
    }
}

