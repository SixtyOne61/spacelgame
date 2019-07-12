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
        
        public bool Hit(ComponentCollision comp)
        {
        	// just check distance
        	if(HitSphere(comp))
        	{
        		// check OBB hit
        		if(HitOBB(comp))
        		{
        			// check perfect hit
        			if(PerfectHit(comp))
        			{
                        // To do
                        return true;
        			}
        		}
        	}
            return false;
        }
        
        public bool HitSphere(ComponentCollision comp)
        {
        	return BBox.HitSphere(comp);
        }
        
        public bool HitOBB(ComponentCollision comp)
        {
        	return BBox.HitOBB(comp);
        }
        
        private bool PerfectHit(ComponentCollision comp)
        {
        	return BBox.PerfectHit(comp);
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
