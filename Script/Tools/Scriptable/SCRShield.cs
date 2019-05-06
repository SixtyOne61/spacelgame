using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    [CreateAssetMenu(fileName = "Scriptable Shield", menuName = "Scriptable/Scriptable Shield")]
    public class SCRShield : ScriptableObject
    {
        public float Duration;
        public float DamageTaken;
    }
}

