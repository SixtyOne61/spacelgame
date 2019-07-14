using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class DynamicZone
    {
        public List<ComponentCollision> _staticObjects = new List<ComponentCollision>();
        public List<ComponentCollision> _dynamicObjects = new List<ComponentCollision>();

        public DynamicZone(ComponentCollision comp)
        {
            _staticObjects.Add(comp);
        }

        public DynamicZone()
        {

        }
        
        public void UpdateStaticCollision()
        {
            foreach (ComponentCollision comp in _dynamicObjects)
            {
                if (comp.Owner.transform.hasChanged)
                {
                    foreach (ComponentCollision other in _staticObjects)
                    {
                        comp.Hit(other);
                    }
                }
            }
        }

        public void UpdateDynamicCollision()
        {
            // to do, collision between dynamic
            for (int i = 0; i < _dynamicObjects.Count; ++i)
            {
                ComponentCollision c1 = _dynamicObjects[i];
                bool c1HasChanged = c1.Owner.transform.hasChanged;
                for (int j = i + 1; j < _dynamicObjects.Count; ++j)
                {
                    ComponentCollision c2 = _dynamicObjects[j];
                    if (!c1HasChanged && !c2.Owner.transform.hasChanged)
                    {
                        continue;
                    }

                    // TO DO
                }
            }
        }
        
        public bool HasContact(DynamicZone other)
        {
            foreach (ComponentCollision otherComp in other._staticObjects)
            {
                if(HasContact(otherComp))
                {
                    return true;
                }
            }
        	return false;
        }

        public bool HasContact(ComponentCollision other)
        {
            foreach (ComponentCollision comp in _staticObjects)
            {
                if (comp.HitSphere(other) && comp.HitOBB(other))
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
                ComponentCollision comp = _dynamicObjects[i];
                if (comp.Owner.transform.hasChanged && !HasContact(comp))
                {
                    CollisionManager.Instance.RegisterDynamic(comp);
                    _dynamicObjects.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }
        }

        public bool UnRegisterStatic(ComponentCollision comp)
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

        public bool UnRegisterDynamic(ComponentCollision comp)
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
    
    
    
