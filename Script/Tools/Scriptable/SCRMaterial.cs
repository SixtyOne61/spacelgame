using UnityEngine;

namespace Tool
{
    [CreateAssetMenu(fileName = "Scriptable Material", menuName = "Scriptable/Scriptable Material")]
    public class SCRMaterial : ScriptableObject
    {
        public Material Material;
    }
}

