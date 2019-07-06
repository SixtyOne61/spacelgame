using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Engine;

public class CollisionManager : Singleton<CollisionManager>
{
	private List<DynamicZone> _dynamicZones = new List<DynamicZone>();
    private DynamicZone _orphanObjects = new DynamicZone();

    [Tooltip("True for disable manager, use for editor scene")]
    public bool IsDisable;

    public void Start()
    {
        // TO DO : maybe add mask for collision between team etc
    }

    #region Register

    public void Register(CompCollisionStatic component)
    {
        // list of all zone who can receive component
        List<DynamicZone> promotZones = new List<DynamicZone>();

        int i = 0;
        while (i < _dynamicZones.Count)
        {
            if(_dynamicZones[i].HasContact(component))
            {
                promotZones.Add(_dynamicZones[i]);
                _dynamicZones.RemoveAt(i);
            }
            else
            {
                ++i;
            }
        }

        // create a new big zone with promotzone
        DynamicZone newZone = new DynamicZone(component);
        foreach (DynamicZone promotZone in promotZones)
        {
            newZone._staticObjects.AddRange(promotZone._staticObjects);
            newZone._dynamicObjects.AddRange(promotZone._dynamicObjects);
        }

        newZone.ComputeInfluenceBox();

        _dynamicZones.Add(newZone);
    }

    public void Register(CompCollisionDynamic component)
    {
        bool atLeastOne = false;
    	for(int i = 0; i < _dynamicZones.Count; ++i)
    	{
            if(_dynamicZones[i].InfluenceBox.HasContact(component.Box))
            {
                _dynamicZones[i]._dynamicObjects.Add(component);
                atLeastOne = true;
            }
        }

        if(!atLeastOne)
        {
            _orphanObjects._dynamicObjects.Add(component);
        }
    }

    #endregion

    #region UnRegister

    public void UnRegister(CompCollisionStatic component)
    {
        // TO DO
    }

    public void UnRegister(CompCollisionDynamic component)
    {
        // TO DO
    }

    #endregion

    public void FixedUpdate()
    {
        foreach (DynamicZone zone in _dynamicZones)
        {
            zone.CheckDynamic();
        }

        // check dynamic for orphan
        // TO DO : create new type zone for this "orphan"

        foreach (DynamicZone zone in _dynamicZones)
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
