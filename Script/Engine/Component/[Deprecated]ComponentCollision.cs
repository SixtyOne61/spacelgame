using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    [System.Serializable]
    public class ComponentCollisionDeprecated : ComponentBase
    {
        [HideInInspector]
        // list of all voxel
        public List<LinkPos> LinkPosList = new List<LinkPos>();

        public BBBase BBox;

        public void Init(float sizeCube)
        {
            if(LinkPosList.Count == 0)
            {
                return;
            }

            BBox = new BBBase(LinkPosList[0], sizeCube, Owner.transform);

            foreach(LinkPos linkPos in LinkPosList)
            {
                BBox.Best(linkPos);
            }
        }
        
        public bool Hit(ComponentCollisionDeprecated comp)
        {
        	// just check distance
        	if(HitSphere(comp))
            {
                Vector3 otherVMin;
                Vector3 otherVMax;
                // check OBB hit
                if (HitOBB(comp, out otherVMin, out otherVMax))
        		{
                    // check perfect hit
                    PerfectHit(comp, otherVMin, otherVMax);
        		}
        	}
            return false;
        }
        
        public bool HitSphere(ComponentCollisionDeprecated comp)
        {
        	return BBox.HitSphere(comp);
        }
        
        public bool HitOBB(ComponentCollisionDeprecated comp)
        {
            Vector3 otherVMin;
            Vector3 otherVMax;
            return HitOBB(comp, out otherVMin, out otherVMax);
        }

        private bool HitOBB(ComponentCollisionDeprecated comp, out Vector3 out_vmin, out Vector3 out_vmax)
        {
        	return BBox.HitOBB(comp, out out_vmin, out out_vmax);
        }
        
        private void PerfectHit(ComponentCollisionDeprecated comp, Vector3 otherVmin, Vector3 otherVmax)
        {
            // find intersect volume
            float minx = Mathf.Max(BBox.Vertex1.x, otherVmin.x);
            float maxx = Mathf.Min(BBox.Vertex7.x, otherVmax.x);

            float miny = Mathf.Max(BBox.Vertex1.y, otherVmin.y);
            float maxy = Mathf.Min(BBox.Vertex7.y, otherVmax.y);

            float minz = Mathf.Max(BBox.Vertex1.z, otherVmin.z);
            float maxz = Mathf.Min(BBox.Vertex7.z, otherVmax.z);

            Vector3 intersectMin = new Vector3(minx, miny, minz);
            Vector3 intersectMax = new Vector3(maxx, maxy, maxz);

            IntersectVolume(intersectMin, intersectMax, comp.Owner.GetComponent<VolumeEntity>().ParamAttribut.Damage);
            
            // convert intersect volume to comp's local space
            intersectMin = Owner.transform.TransformPoint(intersectMin);
            intersectMin = comp.Owner.transform.InverseTransformPoint(intersectMin);
            intersectMax = Owner.transform.TransformPoint(intersectMax);
            intersectMax = comp.Owner.transform.InverseTransformPoint(intersectMax);
            comp.IntersectVolume(intersectMin, intersectMax, Owner.GetComponent<VolumeEntity>().ParamAttribut.Damage);
        }

        public void IntersectVolume(Vector3 vmin, Vector3 vmax, int dmg)
        {
            if(dmg == 0)
            {
                return;
            }

            VolumeEntity ent = Owner.GetComponent<VolumeEntity>();

            for(int i = 0; i < LinkPosList.Count;)
            {
                UnitPos pos = LinkPosList[i].Center;
                if(HasContact(vmin.x, vmax.x, pos.x, pos.x + BBox.SizeCube)
                    && HasContact(vmin.y, vmax.y, pos.y, pos.y + BBox.SizeCube)
                    && HasContact(vmin.z, vmax.z, pos.z, pos.z + BBox.SizeCube))
                {
                    if(ent.RemoveAt(i, dmg))
                    {
                        // TO DO : refresh box
                        continue;
                    }
                }
                ++i;
            }
        }

        private bool HasContact(float min, float max, float start, float end)
        {
            return min >= start ? min <= end : max >= start;
        }

#if (UNITY_EDITOR)
        public override void OnDrawGizmos()
        {
        	// draw box
            Gizmos.color = Color.green;
            Gizmos.DrawLine(BBox.Owner.TransformPoint(BBox.Vertex1.ToVec3()), BBox.Owner.TransformPoint(BBox.Vertex2.ToVec3()));
            Gizmos.DrawLine(BBox.Owner.TransformPoint(BBox.Vertex1.ToVec3()), BBox.Owner.TransformPoint(BBox.Vertex4.ToVec3()));
            Gizmos.DrawLine(BBox.Owner.TransformPoint(BBox.Vertex1.ToVec3()), BBox.Owner.TransformPoint(BBox.Vertex5.ToVec3()));

            Gizmos.DrawLine(BBox.Owner.TransformPoint(BBox.Vertex3.ToVec3()), BBox.Owner.TransformPoint(BBox.Vertex2.ToVec3()));
            Gizmos.DrawLine(BBox.Owner.TransformPoint(BBox.Vertex3.ToVec3()), BBox.Owner.TransformPoint(BBox.Vertex4.ToVec3()));
            Gizmos.DrawLine(BBox.Owner.TransformPoint(BBox.Vertex3.ToVec3()), BBox.Owner.TransformPoint(BBox.Vertex7.ToVec3()));

            Gizmos.DrawLine(BBox.Owner.TransformPoint(BBox.Vertex6.ToVec3()), BBox.Owner.TransformPoint(BBox.Vertex2.ToVec3()));
            Gizmos.DrawLine(BBox.Owner.TransformPoint(BBox.Vertex6.ToVec3()), BBox.Owner.TransformPoint(BBox.Vertex7.ToVec3()));
            Gizmos.DrawLine(BBox.Owner.TransformPoint(BBox.Vertex6.ToVec3()), BBox.Owner.TransformPoint(BBox.Vertex5.ToVec3()));

            Gizmos.DrawLine(BBox.Owner.TransformPoint(BBox.Vertex8.ToVec3()), BBox.Owner.TransformPoint(BBox.Vertex4.ToVec3()));
            Gizmos.DrawLine(BBox.Owner.TransformPoint(BBox.Vertex8.ToVec3()), BBox.Owner.TransformPoint(BBox.Vertex5.ToVec3()));
            Gizmos.DrawLine(BBox.Owner.TransformPoint(BBox.Vertex8.ToVec3()), BBox.Owner.TransformPoint(BBox.Vertex7.ToVec3()));
        }
#endif
    }   
}
