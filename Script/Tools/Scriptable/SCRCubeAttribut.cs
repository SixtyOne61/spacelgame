using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    [CreateAssetMenu(fileName = "Scriptable Cube Attribut", menuName = "Scriptable/Scriptable Cube Attribut")]
    public class SCRCubeAttribut : ScriptableObject
    {
        public int Life;
        public int Damage;
    }
}

