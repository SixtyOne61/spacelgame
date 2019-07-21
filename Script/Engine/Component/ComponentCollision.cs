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

        [Tooltip("Cube size")]
        public float CubeSize;

        public override void Start()
        {
            base.Start();
            // cut linkPosList to small list
            int maxElem = Mathf.RoundToInt(-0.5f + Mathf.Sqrt(0.25f * LinkPosList.Count));
        }
    }
}
