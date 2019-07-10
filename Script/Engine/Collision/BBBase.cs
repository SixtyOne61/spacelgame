using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class BBBase
    {
        private float _sizeCube = 0.0f;

        // list of box position
        [HideInInspector]
        public float xMin = 0.0f;
        [HideInInspector]
        public float xMax = 0.0f;
        [HideInInspector]
        public float yMin = 0.0f;
        [HideInInspector]
        public float yMax = 0.0f;
        [HideInInspector]
        public float zMin = 0.0f;
        [HideInInspector]
        public float zMax = 0.0f;

        BBBase()
        {

        }

        BBBase(LinkPos init, float sizeCube)
        {
            _sizeCube = sizeCube;
            xMin = init.Center.x - _sizeCube;
            xMax = init.Center.x + _sizeCube;

            yMin = init.Center.y - _sizeCube;
            yMax = init.Center.y + _sizeCube;

            zMin = init.Center.z - _sizeCube;
            zMax = init.Center.z + _sizeCube;
        }
    }
}