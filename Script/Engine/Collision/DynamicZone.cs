using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class DynamicZone
    {
        // largest box for this zone
        public BoxParam InfluenceBox;

        public List<CompCollision> _staticObject = new List<CompCollision>();
        public List<CompCollision> _dynamicObject = new List<CompCollision>();

        public DynamicZone(CompCollision comp)
        {
            _staticObject.Add(comp);
            InfluenceBox = comp.Box;
        }
        
        public bool Add(CompCollisionStatic comp)
        {
        	BoxParam box = comp.Box;
        	if(InfluenceBox.HasContact(box))
        	{
        		// check if contact exist with at least one static
        		foreach(CompCollision staticComp in _staticObject)
        		{
        			if(staticComp.Box.HasContact(box))
        			{
        				_staticObject.Add(box);
        				// update relative box
        				UpdateBox();
        				return true;
        			}
        		}
        	}
        	
        	return false;
        }
        
        public bool Add(CompCollisionDynamic comp)
        {
        	BoxParam box = comp.Box;
        	if(InfluenceBox.HasContact(box))
        	{
        		_dynamicObject.Add(comp);
        		return true;
        	}
        	return false;
        }

        public void FusionAddStatic(List<CompCollision> other)
        {
            foreach(CompCollision comp in other)
            {
                _staticObject.Add(comp);
                UpdateBox(comp.Box);
            }
        }

        public void FusionAddDynamic(List<CompCollision> other)
        {
            _dynamicObject.AddRange(other);
        }
        
        private void UpdateBox(BoxParam box)
        {
        	UpdateClamp(ref InfluenceBox.x.Clamp, box.x.Clamp);
        	UpdateClamp(ref InfluenceBox.y.Clamp, box.y.Clamp);
        	UpdateClamp(ref InfluenceBox.z.Clamp, box.z.Clamp);
            // size of cube is 1.0f
            InfluenceBox.Terminate(0.0f);
        }
        
        private void UpdateClamp(ref Vector2 a, Vector2 b)
        {
        	a.x = Mathf.Min(a.x, b.x);
        	a.y = Mathf.Max(a.y, b.y);
        }
        
        public void UpdateCollision()
        {
        	foreach(CompCollision comp in _dynamicObject)
        	{
        		if(comp.Owner.transform.hasChanged)
        		{
        			// To Do collision
        		}
        	}
        }
        
        public bool HasContact(DynamicZone other)
        {
        	foreach(CompCollision comp in _staticObject)
        	{
        		foreach(CompCollision otherComp in other._staticObject)
        		{
        			if(comp.Box.HasContact(otherComp.Box))
        			{
        				return true;
        			}
        		}
        	}
        	return false;
        }

#if (UNITY_EDITOR)
        public void OnDrawGizmos()
        {
            if(Tool.DebugWindowAccess.Instance.Serialize.EnableDrawChunck)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(InfluenceBox.Center, new Vector3(InfluenceBox.x.Clamp.y - InfluenceBox.x.Clamp.x, InfluenceBox.y.Clamp.y - InfluenceBox.y.Clamp.x, InfluenceBox.z.Clamp.y - InfluenceBox.z.Clamp.x));
            }
        }
#endif
    }
}
    
    
    
