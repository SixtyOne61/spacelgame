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

    public override void Start()
    {
        base.Start();

        // spawn removed part
        _mirrorRemoved = Builder.Instance.Build(Builder.FactoryType.Ship, (int)BuilderShip.Type.RemovedShip, transform.position, transform.rotation, transform).GetComponent<EntRemovedShip>();
        _mirrorRemoved.LinkPosList = _removed;
        _mirrorRemoved.CompMeshGenerator.ParamCubeSize = CompMeshGenerator.ParamCubeSize;
    }

    public void Init(Tool.ShipPart part)
    {
        foreach (UnitPos pos in part.Cubes)
        {
            LinkPosList.Add(new LinkPos(pos, ParamAttribut != null ? ParamAttribut.Life : int.MaxValue));
        }

        // init component
        // size of cube
        float size = part.Param.SquareSize;
        CompMeshGenerator.ParamCubeSize = ScriptableObject.CreateInstance(typeof(Tool.SCROneValue)) as Tool.SCROneValue;
        CompMeshGenerator.ParamCubeSize.Value = size;

        CompMaterial.ParamMaterial = ScriptableObject.CreateInstance(typeof(Tool.SCRMaterial)) as Tool.SCRMaterial;
        CompMaterial.ParamMaterial.Material = part.Param.Material;

        ComponentCollision.ParamCubeSize = CompMeshGenerator.ParamCubeSize;
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
        GameObject loot = Builder.Instance.Build(Builder.FactoryType.Fx, (int)Tool.BuilderFx.Type.Loot, transform.TransformPoint(new Vector3(pos.Center.x, pos.Center.y, pos.Center.z)), Quaternion.identity, GameManager.Instance.LootParent);
        loot.GetComponent<EntLoot>().Ressource = ParamAttribut.Life;
    }

    public override void Alive()
    {
        // DO NOTHING, don't destroy
    }
}
