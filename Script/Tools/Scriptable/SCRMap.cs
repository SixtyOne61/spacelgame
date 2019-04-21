﻿using UnityEngine;

namespace Tool
{
    [CreateAssetMenu(fileName = "Scriptable Map", menuName = "Scriptable/Scriptable Map")]
    public class SCRMap : ScriptableObject
    {
        public int Width;
        public int Height;
        public int Depth;
    }
}

