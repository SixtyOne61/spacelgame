﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Engine
{
    [System.Serializable]
    public class CompCollision : ComponentBase
    {
        public Tool.SCROneValue ParamCubeSize;

        [HideInInspector]
        public List<LinkPos> LinkPosList = new List<LinkPos>();

        // use for define box
        public BoxParam Box = new BoxParam();

#if (UNITY_EDITOR)
        // for debug
        private List<BoxParam> _debugPerfectHit = new List<BoxParam>();
#endif

        public override void Start()
        {
            Reset();
            base.Start();
        }

        public void Reset()
        {
            Box.x.IsInit = false;
            Box.y.IsInit = false;
            Box.z.IsInit = false;

            // create LOD box
            foreach (LinkPos pos in LinkPosList)
            {
                Box.x.AddBest(pos.Center.x);
                Box.y.AddBest(pos.Center.y);
                Box.z.AddBest(pos.Center.z);
            }

            Box.Terminate(ParamCubeSize.Value);
        }

        public void Hit(CompCollision other)
        {
            if (HitShpereBox(other))
            {
                if (HitBoundBox(other))
                {
                    PerfectHit(other);
                }
            }
        }

        private bool HitShpereBox(CompCollision other)
        {
            Vector3 otherPos = other.Owner.transform.TransformPoint(other.Box.Center);
            Vector3 pos = Owner.transform.TransformPoint(Box.Center);
            return Vector3.Distance(otherPos, pos) <= (other.Box.Ray + Box.Ray);
        }

        public bool HitBoundBox(CompCollision other)
        {
            return HitBoundBox(other.Owner.transform, other.Box);
        }

        public bool HitBoundBox(Transform otherTransform, BoxParam otherBox)
        {
            Vector3 otherCenter = otherBox.Center;
            otherCenter = otherTransform.transform.TransformPoint(otherCenter);

            // our local space
            otherCenter = Owner.transform.InverseTransformDirection(otherCenter);

            BoxParam tmp = new BoxParam(otherCenter);
            tmp.x.Clamp = new Vector2(otherCenter.x - otherBox.x.Half, otherCenter.x + otherBox.x.Half);
            tmp.y.Clamp = new Vector2(otherCenter.y - otherBox.y.Half, otherCenter.y + otherBox.y.Half);
            tmp.z.Clamp = new Vector2(otherCenter.z - otherBox.z.Half, otherCenter.z + otherBox.z.Half);

            return Box.HasContact(tmp);
        }

        private void PerfectHit(CompCollision other)
        {
            BoxParam tmp = GetRelativeBox(Box, this, other.Box, other);
            BoxParam tmp2 = GetRelativeBox(other.Box, other, Box, this);

            // size of each cube
            float size = ParamCubeSize.Value;
            float otherSize = other.ParamCubeSize.Value;

            // we need to remove on each side if we have contact between box1 and box2
            List<int> toRemove = new List<int>();
            FindPerfectContact(ref LinkPosList, ref tmp, size, ref toRemove);

            if (toRemove.Count == 0)
            {
                return;
            }

            List<int> toRemoveOther = new List<int>();
            FindPerfectContact(ref other.LinkPosList, ref tmp2, otherSize, ref toRemoveOther);

            if (toRemoveOther.Count == 0)
            {
                return;
            }

            RemovePoint(toRemove, Owner, other.Owner);
            RemovePoint(toRemoveOther, other.Owner, Owner);

#if (UNITY_EDITOR)
            if (Tool.DebugWindowAccess.Instance.Serialize.EnableDrawRelativeBoxCollision)
            {
                tmp.Center = other.Owner.transform.TransformPoint(tmp.Center);
                _debugPerfectHit.Add(tmp);
                tmp2.Center = Owner.transform.TransformPoint(tmp2.Center);
                _debugPerfectHit.Add(tmp2);
            }
#endif
        }

        private void RemovePoint(List<int> removeList, GameObject owner, GameObject other)
        {
            int dmg = other.GetComponent<VolumeEntity>().ParamAttribut.Damage;
            int delta = 0;
            VolumeEntity ent = owner.GetComponent<VolumeEntity>();
            foreach (int idx in removeList)
            {
                ent.RemoveAt(idx - delta, dmg);
                ++delta;
            }
        }

        private BoxParam GetRelativeBox(BoxParam box1, CompCollision comp1, BoxParam box2, CompCollision comp2)
        {
            // transform center of box2 to world coord
            Vector3 otherCenter = comp2.Box.Center;
            otherCenter = comp2.Owner.transform.TransformPoint(otherCenter);

            // transform to box1 local coord
            otherCenter = comp1.Owner.transform.InverseTransformPoint(otherCenter);

            // define volume intersect
            BoxParam tmp = new BoxParam();
            tmp.x.Clamp = box1.x.GetVolumeContact(new Vector2(otherCenter.x - box2.x.Half, otherCenter.x + box2.x.Half));
            tmp.y.Clamp = box1.y.GetVolumeContact(new Vector2(otherCenter.y - box2.y.Half, otherCenter.y + box2.y.Half));
            tmp.z.Clamp = box1.z.GetVolumeContact(new Vector2(otherCenter.z - box2.z.Half, otherCenter.z + box2.z.Half));

            // compute data
            tmp.Terminate(0.0f);

            return tmp;
        }

        private void FindPerfectContact(ref List<LinkPos> list, ref BoxParam intersectVolume, float size, ref List<int> toRemove)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                UnitPos pos = list.ElementAt(i).Center;
                if (intersectVolume.x.HasContact(pos.x, pos.x + size)
                    && intersectVolume.y.HasContact(pos.y, pos.y + size)
                    && intersectVolume.z.HasContact(pos.z, pos.z + size))
                {
                    toRemove.Add(i);
                }
            }
        }

#if (UNITY_EDITOR)
        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (Tool.DebugWindowAccess.Instance.Serialize.EnableDrawRelativeBoxCollision)
            {
                Gizmos.color = Color.green;
                foreach (BoxParam box in _debugPerfectHit)
                {
                    Gizmos.DrawCube(box.Center, new Vector3(box.x.Size, box.y.Size, box.z.Size));
                }
            }

            if (Tool.DebugWindowAccess.Instance.Serialize.EnableDrawChunck)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(Box.Center, new Vector3(Box.x.Clamp.y - Box.x.Clamp.x, Box.y.Clamp.y - Box.y.Clamp.x, Box.z.Clamp.y - Box.z.Clamp.x));
            }
        }
#endif
    }

}