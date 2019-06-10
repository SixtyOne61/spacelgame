using UnityEngine;

namespace Tool
{
    [CreateAssetMenu(fileName = "Scriptable Noise", menuName = "Scriptable/Whale/Scriptable Noise")]
    public class SCRNoise : ScriptableObject
    {
        public Vector2 Threshold;
        [Tooltip("param, each Volume entity can make damage")]
        public SCRCubeAttribut ParamAttribut;
        [Tooltip("Cube size")]
        public SCROneValue ParamCubeSize;
        [Tooltip("Material param")]
        public SCRMaterial ParamMaterial;
    }
}

