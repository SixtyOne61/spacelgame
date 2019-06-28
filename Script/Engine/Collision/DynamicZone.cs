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
        	}
        }
        
        private void UpdateBox(BoxParam box)
        {
        	UpdateClamp(InfluenceBox.x.Clamp, box.x.Clamp);
        	UpdateClamp(InfluenceBox.y.Clamp, box.y.Clamp);
        	UpdateClamp(InfluenceBox.z.Clamp, box.z.Clamp);
        	InfluenceBox.Terminate();
        }
        
        private void UpdateClamp(Vector2 a, Vector2 b)
        {
        	a.x = Min(a.x, b.x);
        	a.y = Max(a.y, b.y);
        }
    }
}
    
