using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class ComponentMeshBase : ComponentBase
    {
        [HideInInspector]
        public Mesh CustomMesh;

        // Use this for initialization
        override public void Start()
        {
            CustomMesh = new Mesh();
        }
    }

}
