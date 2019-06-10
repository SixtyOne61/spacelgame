using UnityEngine;
using System;

[System.Serializable]
public class UnitPos : IComparable
{
    public static UnitPos UPINVALID = new UnitPos(float.NaN, float.NaN, float.NaN);

    public float x;
    public float y;
    public float z;

    private float _storeDist = float.NaN;

    public UnitPos(float ax, float ay, float az)
    {
        x = ax;
        y = ay;
        z = az;
    }

    public UnitPos(UnitPos aPos)
    {
        x = aPos.x;
        y = aPos.y;
        z = aPos.z;
    }

    public Vector3 ToVec3()
    {
        return new Vector3(x, y, z);
    }

    public override bool Equals(object obj)
    {
        if(obj == null)
            return false;

        UnitPos o = obj as UnitPos;

        if (o == null)
            return false;

        return Mathf.Abs(x - o.x) < 0.05f
            && Mathf.Abs(y - o.y) < 0.05f
            && Mathf.Abs(z - o.z) < 0.05f;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static bool operator ==(UnitPos c1, UnitPos c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(UnitPos c1, UnitPos c2)
    {
        return !c1.Equals(c2);
    }

    public static UnitPos operator +(UnitPos c1, Vector3 c2)
    {
        c1.x += c2.x;
        c1.y += c2.y;
        c1.z += c2.z;
        return c1;
    }

    public override string ToString()
    {
        return "(" + x.ToString("f3") + ", " + y.ToString("f3") + ", " + z.ToString("f3") + ")";
    }

    public float DistTo0()
    {
        if(_storeDist == float.NaN)
        {
            _storeDist = x * x + y * y + z * z;
        }
        return _storeDist;
    }

    public int CompareTo(object other)
    {
        // A null value means that this object is greater.
        if (other == null)
            return 1;

        UnitPos otherUnitPos = other as UnitPos;
        if(otherUnitPos != null)
        {
            return DistTo0() > otherUnitPos.DistTo0() ? 1 : DistTo0() == otherUnitPos.DistTo0() ? 0 : -1;
        }

        return 1;
    }
}
