using UnityEngine;

namespace Tool
{
    [CreateAssetMenu(fileName = "Scriptable Whale", menuName = "Scriptable/Whale/Scriptable Whale")]
    public class SCRWhale : ScriptableObject
    {
    	[Tooltip("Size of chunck")]
    	public int SizeChunck;
    	[Tooltip("Number of chunck")]
        public int NbChunck;
        public Vector2Int Width = SizeChunck * NbChunck;
        public Vector2Int Height = SizeChunck * NbChunck;
        public Vector2Int Depth = SizeChunck * NbChunck;
        
        
    }
}

