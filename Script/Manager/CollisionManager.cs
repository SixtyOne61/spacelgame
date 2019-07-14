using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Engine;

public class CollisionManager : Singleton<CollisionManager>
{
	private List<DynamicZone> _dynamicZones = new List<DynamicZone>();
    private DynamicOrphanZone _orphanObjects = new DynamicOrphanZone();

    [Tooltip("True for disable manager, use for editor scene")]
    public bool IsDisable;

    public void Start()
    {
        // TO DO : maybe add mask for collision between team etc
    }
    
    public bool AddDynamic(ComponentCollision comp)
    {
        bool ret = false;
    	foreach(DynamicZone zone in _dynamicZones)
    	{
            if(zone.HasContact(comp))
            {
                zone._dynamicObjects.Add(comp);
                ret = true;
            }
    	}
    	
    	return ret;
    }

    #region Register

    public void RegisterStatic(ComponentCollision component)
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

        _dynamicZones.Add(newZone);
    }

    public void RegisterDynamic(ComponentCollision component)
    {
        if(!AddDynamic(component))
        {
            _orphanObjects._dynamicObjects.Add(component);
        }
    }

    #endregion

    #region UnRegister

    public void UnRegisterStatic(ComponentCollision component)
    {
        foreach(DynamicZone zone in _dynamicZones)
        {
            if(zone.UnRegisterStatic(component))
            {
                break;
            }
        }
    }

    public void UnRegisterDynamic(ComponentCollision component)
    {
        foreach (DynamicZone zone in _dynamicZones)
        {
            zone.UnRegisterDynamic(component);
        }

        _orphanObjects.UnRegisterDynamic(component);
    }

    #endregion

    public void FixedUpdate()
    {
        /*foreach (DynamicZone zone in _dynamicZones)
        {
            zone.CheckDynamic();
        }*/

        // check if orphan is still orphan
        _orphanObjects.CheckDynamic();
        
        foreach (DynamicZone zone in _dynamicZones)
        {
            zone.UpdateStaticCollision();
            zone.UpdateDynamicCollision();
        }

        _orphanObjects.UpdateDynamicCollision();
    }
}
