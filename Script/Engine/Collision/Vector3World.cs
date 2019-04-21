using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class Vector3World
    {

        private Vector3 _base;

        public Vector3World(Vector3 val)
        {
            _base = val;
        }

        public Vector3 ToWorld(Transform trRef)
        {
            return trRef.TransformPoint(_base);
        }
    }
}