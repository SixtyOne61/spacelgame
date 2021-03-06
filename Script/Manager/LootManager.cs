﻿using System.Collections.Generic;
using UnityEngine;
using Tool;

public class LootManager : Singleton<LootManager>
{
	[HideInInspector]
	// loot parent, contain all loot
	public Transform LootParent;
	
	[Tooltip("Range of loot")]
	public SCROneValue ParamRange;
	
	public void Start()
	{
		// spawn empty parent
		LootParent = Builder.SpawnEmpty("Loot parent").transform;
        LootParent.parent = transform;
	}
	
	public void AddLoot(Vector3 position, int ressource) // to do team
	{
		// check if we have loot near
		int nbChild = LootParent.childCount;
		for(int i = 0; i < nbChild; ++i)
		{
			Transform child = LootParent.GetChild(i);
			if(Vector3.Distance(position, child.position) < ParamRange.Value)
			{
				// refill this loot
				child.GetComponent<EntLoot>().Ressource += ressource;
				return;
			}
		}
		
		// spawn new loot
		GameObject loot = Builder.Instance.Build(Builder.FactoryType.Fx, (int)Tool.BuilderFx.Type.Loot, position, Quaternion.identity, LootParent);
		loot.GetComponent<EntLoot>().Ressource = ressource;		
	}
}
    
