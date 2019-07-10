using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class DynamicZone
    {
        public List<CompCollision> _staticObjects = new List<CompCollision>();
        public List<CompCollision> _dynamicObjects = new List<CompCollision>();

        public DynamicZone(CompCollision comp)
        {
            _staticObjects.Add(comp);
        }

        public DynamicZone()
        {

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
        	
        	// to do, collision between dynamic
        	for(int i = 0; i < _dynamicObjects.Count; ++i)
        	{
                CompCollision c1 = _dynamicObjects[i];
        		bool c1HasChanged = c1.Owner.transform.hasChanged;
        		for(int j = i + 1; j < _dynamicObjects.Count; ++j)
        		{
                    CompCollision c2 = _dynamicObjects[j];
        			if(!c1HasChanged && !c2.Owner.transform.hasChanged)
        			{
        				continue;
        			}

                    // TO DO
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
                if (comp.HitBoundBox(other))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void CheckDynamic()
        {
            for(int i = 0; i < _dynamicObjects.Count; )
            {
                CompCollision comp = _dynamicObjects[i];
                if (comp.Owner.transform.hasChanged && !HasContact(comp))
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

        public bool UnRegisterStatic(CompCollision comp)
        {
            for(int i = 0; i < _staticObjects.Count; ++i)
            {
                if(comp.GetHashCode() == _staticObjects[i].GetHashCode())
                {
                    _staticObjects.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public bool UnRegisterDynamic(CompCollision comp)
        {
            for(int i = 0; i < _dynamicObjects.Count; ++i)
            {
                if(comp.GetHashCode() == _dynamicObjects[i].GetHashCode())
                {
                    _dynamicObjects.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
    }
}
    
    
    
