using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class DynamicZone
    {
        // largest box for this zone
        public BoxParam InfluenceBox;

        public List<CompCollision> _staticObjects = new List<CompCollision>();
        public List<CompCollision> _dynamicObjects = new List<CompCollision>();

        public DynamicZone(CompCollision comp)
        {
            _staticObjects.Add(comp);
        }

        public DynamicZone()
        {

        }

        public void ComputeInfluenceBox()
        {
            if(_staticObjects.Count == 0)
            {
                Debug.LogError("[DynamicZone] Compute Influence box, but without _static Object.");
                return;
            }

            InfluenceBox = new BoxParam(_staticObjects[0].Box);

            foreach(CompCollision staticObject in _staticObjects)
            {
                UpdateBox(staticObject.Box);
            }
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
        	foreach(CompCollision comp in _dynamicObjects)
        	{
        		if(comp.Owner.transform.hasChanged)
        		{
        			foreach(CompCollision other in _staticObjects)
                    {
                        comp.Hit(other);
                    }
        		}
        	}
        }
        
        public bool HasContact(DynamicZone other)
        {
            foreach (CompCollision otherComp in other._staticObjects)
            {
                if(HasContact(otherComp))
                {
                    return true;
                }
            }
        	return false;
        }

        public bool HasContact(CompCollision other)
        {
            foreach (CompCollision comp in _staticObjects)
            {
                if (comp.Box.HasContact(other.Box))
                {
                    return true;
                }
            }
            return false;
        }

        public void CheckDynamic()
        {
            for(int i = 0; i < _dynamicObjects.Count; )
            {
                CompCollision comp = _dynamicObjects[i];
                if (comp.Owner.transform.hasChanged && !comp.Box.HasContact(InfluenceBox))
                {
                    CollisionManager.Instance.Register((CompCollisionDynamic)comp);
                    _dynamicObjects.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }
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
    
    
    
