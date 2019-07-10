using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class BBBase
    {
        protected float _sizeCube = 0.0f;

        // transform of Owner
        [HideInInspector]
        public Transform Owner;

        // all vertex
        [HideInInspector]
        public UnitPos Vertex1;
        [HideInInspector]
        public UnitPos Vertex2;
        [HideInInspector]
        public UnitPos Vertex3;
        [HideInInspector]
        public UnitPos Vertex4;
        [HideInInspector]
        public UnitPos Vertex5;
        [HideInInspector]
        public UnitPos Vertex6;
        [HideInInspector]
        public UnitPos Vertex7;
        [HideInInspector]
        public UnitPos Vertex8;

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

        public BBBase()
        {

        }

        public BBBase(LinkPos init, float sizeCube, Transform owner)
        {
            Owner = owner;

            UnitPos center = init.Center;
            _sizeCube = sizeCube;

            xMin = center.x - _sizeCube;
            xMax = center.x + _sizeCube;

            yMin = center.y - _sizeCube;
            yMax = center.y + _sizeCube;

            zMin = center.z - _sizeCube;
            zMax = center.z + _sizeCube;

            Vertex1 = new UnitPos(xMin, yMin, zMin);
            Vertex2 = new UnitPos(xMax, yMin, zMin);
            Vertex3 = new UnitPos(xMax, yMax, zMin);
            Vertex4 = new UnitPos(xMin, yMax, zMin);
            Vertex5 = new UnitPos(xMin, yMin, zMax);
            Vertex6 = new UnitPos(xMax, yMin, zMax);
            Vertex7 = new UnitPos(xMax, yMax, zMax);
            Vertex8 = new UnitPos(xMin, yMax, zMax);
        }

        public void Best(LinkPos pos)
        {
            UnitPos center = pos.Center;

            if (center.x - _sizeCube < xMin)
            {
                xMin = center.x - _sizeCube;
                Vertex1.x = xMin;
                Vertex4.x = xMin;
                Vertex5.x = xMin;
                Vertex8.x = xMin;
            }
            else if (center.x + _sizeCube > xMax)
            {
                xMax = center.x + _sizeCube;
                Vertex2.x = xMax;
                Vertex3.x = xMax;
                Vertex6.x = xMax;
                Vertex7.x = xMax;
            }

            if (center.y - _sizeCube < yMin)
            {
                yMin = center.y - _sizeCube;
                Vertex1.y = yMin;
                Vertex2.y = yMin;
                Vertex5.y = yMin;
                Vertex6.y = yMin;
            }
            else if (center.y + _sizeCube > yMax)
            {
                yMax = center.y + _sizeCube;
                Vertex3.y = yMax;
                Vertex4.y = yMax;
                Vertex7.y = yMax;
                Vertex8.y = yMax;
            }

            if (center.z - _sizeCube < zMin)
            {
                zMin = center.z - _sizeCube;
                Vertex1.z = zMin;
                Vertex2.z = zMin;
                Vertex3.z = zMin;
                Vertex4.z = zMin;
            }
            else if (center.z + _sizeCube > zMax)
            {
                zMax = center.z + _sizeCube;
                Vertex5.z = zMax;
                Vertex6.z = zMax;
                Vertex7.z = zMax;
                Vertex8.z = zMax;
            }
        }

        public bool HitOBB(ComponentCollision comp)
        {
            // aabb from comp
            BBBase b2 = comp.BBox;
            // transform com Vertex1 (full min) and Vertex7 (full max) to our local space
            UnitPos v1 = b2.Vertex1;
            UnitPos v7 = b2.Vertex7;

            Vector3 v1World = b2.Owner.TransformPoint(new Vector3(v1.x, v1.y, v1.z));
            Vector3 v7World = b2.Owner.TransformPoint(new Vector3(v7.x, v7.y, v7.z));

            Vector3 v1Local = Owner.InverseTransformPoint(v1World);
            Vector3 v7Local = Owner.InverseTransformPoint(v7World);

            return HitAABB(v1Local, v7Local);
        }

        public bool HitAABB(Vector3 min, Vector3 max)
        {
            return Vertex1.x >= min.x ? Vertex1.x <= max.x : Vertex7.x >= min.x
                && Vertex1.y >= min.y ? Vertex1.y <= max.y : Vertex7.y >= min.y
                && Vertex1.z >= min.z ? Vertex1.z <= max.z : Vertex7.z >= min.z;
        }
    }
}