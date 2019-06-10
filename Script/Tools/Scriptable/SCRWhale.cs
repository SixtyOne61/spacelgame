using UnityEngine;

namespace Tool
{
    [CreateAssetMenu(fileName = "Scriptable Whale", menuName = "Scriptable/Whale/Scriptable Whale")]
    public class SCRWhale : ScriptableObject
    {
        public Vector2Int Width;
        public Vector2Int Height;
        public Vector2Int Depth;
    }
}

