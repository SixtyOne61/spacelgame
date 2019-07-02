using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Engine;

public class CollisionManager : Singleton<CollisionManager>
{
    // To do : order world collision separate in zone for reduce number of call
    //private CollisionGroup<CompCollisionPlayer> _playerGroup = new CollisionGroup<CompCollisionPlayer>();
    //private CollisionGroup<CompCollisionWorld> _worldGroup = new CollisionGroup<CompCollisionWorld>();
    //private CollisionGroup<CompCollisionBullet> _bulletGroup = new CollisionGroup<CompCollisionBullet>();
    //private CollisionGroup<CompShield> _shieldGroup = new CollisionGroup<CompShield>();

    private List<DynamicZone> _dynamicZones = new List<DynamicZone>();

    [Tooltip("True for disable manager, use for editor scene")]
    public bool IsDisable;

    // optim all static zone on next update
    public bool OptimStaticZone = false;


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
            if (_dynamicZones[i].AddStatic(component))
            {
            	// find if this zone has contact with an other
            	//ConcatZone(_dynamicZones[i], i);
                return;
            }
        }
        _dynamicZones.Add(new DynamicZone(component));
    }

    public void Register(CompCollisionDynamic component)
    {
    	for(int i = 0; i < _dynamicZones.Count; ++i)
    	{
            _dynamicZones[i].Add(component);

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
    
    private void ConcatZone(DynamicZone zone, int idx)
    {
    	for(int i = 0; i < _dynamicZones.Count; )
    	{
    		if(i == idx)
    		{
    			++i;
    			continue;
    		}
    		
    		if(zone.InfluenceBox.HasContact(_dynamicZones[i].InfluenceBox))
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
