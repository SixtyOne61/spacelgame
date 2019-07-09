using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class DynamicOrphanZone : DynamicZone
    {
    	public override void CheckDynamic()
    	{
    		for(int i = 0; i < _dynamicObjects.Count; )
    		{
    			if(CollisionManager.Instance.AddDynamic(_dynamicObjects[i]))
    			{
    				_dynamicObjects.RemoveAt(i);
    			}
    			else
    			{
    				++i;
    			}
    		}
    	}
    }
}
