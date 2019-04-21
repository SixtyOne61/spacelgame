using UnityEngine;

//[Deprecated] rename
namespace Tool
{
    public enum TypeCube : int
    {
        Default = 0,
        Body,
        Gun,
        Cockpit,
        Cloud,
        Rock,
        Power,
        Bullet,

        Count
    }

    [CreateAssetMenu(fileName = "Scriptable Cube", menuName = "Scriptable/Scriptable Cube")]
    public class ScriptableCube : ScriptableObject
    {
        public string UniqueName;
        public TypeCube Type;
        public Color DebugColor;
        public float SquareSize;
        public Material Material;
        public bool IsOptimizeMesh;

        public int GetUniqueId() { return UniqueName.GetHashCode(); }
    }
}

