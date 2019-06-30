using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Engine;
using Tool;

public class EntWorld : SpacelEntity
{
    public override void Start()
    {
        base.Start();
        Spawn();
    }
    
    public void Spawn()
    {
        // spawn all prefabs in Ressources folder
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Generate");
    	foreach(GameObject go in prefabs)
    	{
    		GameObject chunck = Instantiate(go, Vector3.zero, Quaternion.identity);
    		chunck.transform.SetParent(transform);
    	}

        CollisionManager.Instance.OptimStaticZone = true;
    }
}
    
