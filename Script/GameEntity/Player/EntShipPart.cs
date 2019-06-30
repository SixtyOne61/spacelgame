using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Engine;
using Tool;

public class EntShipPart : CollideEntity
{
    private List<LinkPos> _removed = new List<LinkPos>();
    private EntRemovedShip _mirrorRemoved;
    
    // true if we still have an alive cube
    [HideInInspector]
    public bool PartIsAlive = true;
    
    // min and max on each axis
    [HideInInspector]
    public Vector2 X;
    public Vector2 Y;
    public Vector2 Z;

    public override void Start()
    {
        IsStaticObject = false;
        base.Start();

        // spawn removed part
        _mirrorRemoved = Builder.Instance.Build(Builder.FactoryType.Ship, (int)BuilderShip.Type.RemovedShip, transform.position, transform.rotation, transform).GetComponent<EntRemovedShip>();
        _mirrorRemoved.LinkPosList = _removed;
        _mirrorRemoved.CompMeshGenerator.ParamCubeSize = CompMeshGenerator.ParamCubeSize;
    }

    public void Init(Tool.ShipPart part)
    {
    	UnitPos posInit = part.Cubes[0];
    	X = new Vector2(posInit.x, posInit.x);
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

    public override void RemoveAt(int index, int dmg)
    {
        LinkPos remove = LinkPosList.ElementAt(index);

        remove.Life -= dmg;

        // we don't remove this component, we have life
        if (remove.Life > 0)
        {
            return;
        }

        // spawn a loot on this pos
        SpawnLoot(remove);

        LinkPosList.RemoveAt(index);
        _flagRefresh = true;

        // for rebuild
        _removed.Add(remove);
        // for generic debug
        _debugRemove.Add(remove);
    }

    private void SpawnLoot(LinkPos pos)
    {
    	Vector3 worldPos =  transform.TransformPoint(new Vector3(pos.Center.x, pos.Center.y, pos.Center.z));
    	LootManager.Instance.AddLoot(worldPos, ParamAttribut.Life);
    }

    public override void Alive()
    {
        // DO NOTHING, don't destroy
        PartIsAlive = false;
    }
}
