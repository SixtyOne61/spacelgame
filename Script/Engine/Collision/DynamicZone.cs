using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class DynamicZone
    {
        // largest box for this zone
        public BoxParam InfluenceBox;

        private List<CollideEntity> _staticObject = new List<CollideEntity>();
        private List<CollideEntity> _dynamicObject = new List<CollideEntity>();

        public DynamicZone(CollideEntity entity)
        {
            _staticObject.Add(entity);
            InfluenceBox = entity.ComponentCollision.Box;
        }
        
        public bool AddStatic(CollideEntity entity)
        {
        	BoxParam box = entity.ComponentCollision.Box;
        	
        	float distance = Vector3.Distance(InfluenceBox.Center, box.Center);
        	distance -= InfluenceBox.LargeSize;
        	distance -= box.LargeSize;
        	
        	if(distance <= 0.0f)
        	{
        		_staticObject.Add(entity);
        		// update box
        		UpdateBox(box);
                return true;
        	}

            return false;
        }
        
        private void UpdateBox(BoxParam box)
        {
        	UpdateClamp(InfluenceBox.x.Clamp, box.x.Clamp);
        	UpdateClamp(InfluenceBox.y.Clamp, box.y.Clamp);
        	UpdateClamp(InfluenceBox.z.Clamp, box.z.Clamp);
            // size of cube is 1.0f
        	InfluenceBox.Terminate(1.0f);
        }
        
        private void UpdateClamp(Vector2 a, Vector2 b)
        {
        	a.x = Mathf.Min(a.x, b.x);
        	a.y = Mathf.Max(a.y, b.y);
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
    
