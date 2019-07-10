using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    [System.Serializable]
    public class ComponentCollision : ComponentBase
    {
        [HideInInspector]
        // list of all voxel
        public List<LinkPos> LinkPosList = new List<LinkPos>();


    }
}