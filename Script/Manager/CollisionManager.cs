using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Engine;

public class CollisionManager : Singleton<CollisionManager>
{
	private List<DynamicZone> _dynamicZones = new List<DynamicZone>();

    [Tooltip("True for disable manager, use for editor scene")]
    public bool IsDisable;

    public void Start()
    {
        // TO DO : maybe add mask for collision between team etc
    }

    #region Register

    private bool CanRegister()
    {
        if (IsDisable)
        {
            return false;
        }

        // disable on editor
        if (SceneManager.GetActiveScene().name.GetHashCode() == "Editor".GetHashCode())
        {
            return false;
        }

        return true;
    }

    public void Register(CompCollisionStatic component)
    {
    	for(int i = 0; i < _dynamicZones.Count; ++i)
        {
            if (_dynamicZones[i].Add(component))
            {
            	// find if this zone is around an other
            	EatZone(_dynamicZones[i], i);
                return;
            }
        }
        _dynamicZones.Add(new DynamicZone(component));
    }

    public void Register(CompCollisionDynamic component)
    {
    	for(int i = 0; i < _dynamicZones.Count; ++i)
    	{
            if(_dynamicZones[i].Add(component))
            {
            	return;
            }
        }
    	// To do manage dynamic object alone
    }

    #endregion

    #region UnRegister

    public void UnRegister(CompCollision component)
    {
        // TO DO
    }
    
    #endregion
    
    private void EatZone(DynamicZone zone, int idx)
    {
    	for(int i = 0; i < _dynamicZones.Count; )
    	{
    		if(i == idx)
    		{
    			++i;
    			continue;
    		}
    		
    		if(zone.HasContact(_dynamicZones(i)))
    		{
    			zone.FusionAddStatic(_dynamicZones[i]._staticObject);
    			zone.FusionAddDynamic(_dynamicZones[i]._dynamicObject);
               _dynamicZones.RemoveAt(i);
    		}
    		else
    		{
    			++i;
    		}
    	}
    }

    public void FixedUpdate()
    {
        foreach(DynamicZone zone in _dynamicZones)
        {
        	zone.UpdateCollision();
        }
    }

#if (UNITY_EDITOR)
    public void OnDrawGizmos()
    {
        foreach(Engine.DynamicZone dynamicZone in _dynamicZones)
        {
            dynamicZone.OnDrawGizmos();
        }
    }
#endif

}
