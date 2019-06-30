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
        foreach (DynamicZone dynamicZone in _dynamicZones)
        {
            if (dynamicZone.AddStatic(component))
            {
                return;
            }
        }
        _dynamicZones.Add(new DynamicZone(component));
    }

    public void Register(CompCollisionDynamic component)
    {

    }

    #endregion

    #region UnRegister

    public void UnRegister(CompCollision component)
    {
        // TO DO
    }
    
    #endregion

    public void FixedUpdate()
    {
        /*if(OptimStaticZone)
        {
            OptimStaticZone = false;
            for (int i = 0; i < _dynamicZones.Count; ++i)
            {
                DynamicZone refDynamicZone = _dynamicZones[i];
                for(int j = 0; j < _dynamicZones.Count;)
                {
                    if(i == j)
                    {
                        ++i;
                        continue;
                    }

                    if(refDynamicZone.InfluenceBox.HasContact(_dynamicZones[j].InfluenceBox))
                    {
                        refDynamicZone.FusionAddStatic(_dynamicZones[j]._staticObject);
                        refDynamicZone.FusionAddDynamic(_dynamicZones[j]._dynamicObject);
                        _dynamicZones.RemoveAt(j);
                    }
                    else
                    {
                        ++j;
                    }
                }
            }
        }*/
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
