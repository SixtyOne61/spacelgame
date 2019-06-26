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
        	
        }
    }
}
