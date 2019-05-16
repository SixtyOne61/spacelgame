using System.Collections.Generic;
using UnityEngine;
using Tool;

public class LootManager : Singleton<LootManager>
{
	[HideInInspector]
	// loot parent, contain all loot
	public Transform LootParent;
	
	[Tooltip("Range of loot")]
	SRCOneValue ParamRange;
	
	public void Start()
	{
		// spawn empty parent
		LootParent = Builder.Instance.SpawnEmpty("Loot parent").transform;
	}
	
	public void AddLoot(Vector3 position) // to do team
	{
		// check if we have loot near
		int nbChild = transform.childCount;
		for(int i = 0; i < nbChild; ++i)
		{
			if(Vector3.distance(position, transform.GetChild(i).position))
			{
				// refill this looy
				return;
			}
			else
			{
				// spawn new loot
			}
		}
	}
}
