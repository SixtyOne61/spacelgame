using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class VisuelEntity : SpacelEntity
    {
        //[Deprecated] just remove _mesh, but we need to find a solution for EntBorder
        protected Mesh _mesh;

        public override void Start()
        {
            base.Start();

            _mesh = new Mesh();
        }
    }
}