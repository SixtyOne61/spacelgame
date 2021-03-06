﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Engine;
using Tool;

public class EntShipPart : VolumeEntity
{
    private List<LinkPos> _removed = new List<LinkPos>();
    private EntRemovedShip _mirrorRemoved;
    
    // true if we still have an alive cube
    [HideInInspector]
    public bool PartIsAlive = true;
    
    // min and max on each axis
    [HideInInspector]
    public Vector2 X;
    [HideInInspector]
    public Vector2 Y;
    [HideInInspector]
    public Vector2 Z;
    
    public EntPlayer PlayerOwner;

    public override void Start()
    {
        base.Start();

        // spawn removed part
        _mirrorRemoved = Builder.Instance.Build(Builder.FactoryType.Ship, (int)BuilderShip.Type.RemovedShip, transform.position, transform.rotation, transform).GetComponent<EntRemovedShip>();
        _mirrorRemoved.LinkPosList = _removed;
        _mirrorRemoved.CompMeshGenerator.ParamCubeSize = CompMeshGenerator.ParamCubeSize;

        PlayerOwner.PartSpawned();
    }

    public void Init(Tool.ShipPart part)
    {
    	UnitPos posInit = part.Cubes[0];
    	//X = new Vector2(posInit.x, posInit.x);
    	Y = new Vector2(posInit.y, posInit.y);
    	Z = new Vector2(posInit.z, posInit.z);
    	
        foreach (UnitPos pos in part.Cubes)
        {
            LinkPosList.Add(new LinkPos(pos, ParamAttribut != null ? ParamAttribut.Life : int.MaxValue));
            // update min and max
            X.x = Mathf.Min(X.x, pos.x);
            X.y = Mathf.Max(X.y, pos.x);
            
            Y.x = Mathf.Min(Y.x, pos.y);
            Y.y = Mathf.Max(Y.y, pos.y);
            
            Z.x = Mathf.Min(Z.x, pos.z);
            Z.y = Mathf.Max(Z.y, pos.z);
        }

        // init component
        // size of cube
        float size = part.Param.SquareSize;
        CompMeshGenerator.ParamCubeSize = ScriptableObject.CreateInstance(typeof(Tool.SCROneValue)) as Tool.SCROneValue;
        CompMeshGenerator.ParamCubeSize.Value = size;

        CompMaterial.ParamMaterial = ScriptableObject.CreateInstance(typeof(Tool.SCRMaterial)) as Tool.SCRMaterial;
        CompMaterial.ParamMaterial.Material = part.Param.Material;
    }

    public override bool RemoveAt(int _index, ref LinkPos _remove)
    {
        base.RemoveAt(_index, ref _remove);
        // spawn a loot on this pos
        SpawnLoot(_remove);

        // for rebuild
        _removed.Add(_remove);
        // for generic debug
        _debugRemove.Add(_remove);
        return true;
    }

    private void SpawnLoot(LinkPos pos)
    {
    	Vector3 worldPos = transform.TransformPoint(new Vector3(pos.Center.x, pos.Center.y, pos.Center.z));
    	LootManager.Instance.AddLoot(worldPos, ParamAttribut.Life);
    }

    public override void Alive()
    {
        // DO NOTHING, don't destroy
        PartIsAlive = false;
    }
}
