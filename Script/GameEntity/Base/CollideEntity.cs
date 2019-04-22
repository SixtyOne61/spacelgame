using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using System.Linq;

public class CollideEntity : VolumeEntity
{
    // component collision needed
    public CompCollision ComponentCollision;
    // param, each collide entity can make damage
    public Tool.SCRCubeAttribut ParamAttribut;

    // debug
    protected List<LinkPos> _debugRemove = new List<LinkPos>();

    public override void Start()
    {
        ComponentCollision.LinkPosList = LinkPosList;
        AddComponent(ComponentCollision);
        base.Start();
        CollisionManager.Instance.Register(ComponentCollision, gameObject.tag.GetHashCode());
    }

    public virtual void RemoveAt(int index, int dmg)
    {
        LinkPos remove = LinkPosList.ElementAt(index);

        remove.Life -= dmg;

        // we don't remove this component, we have life
        if(remove.Life > 0)
        {
            return;
        }

        foreach (KeyValuePair<LinkPos.Neighbor, UnitPos> neigbor in remove.Neighbors)
        {
            int invert = (int)neigbor.Key * -1;
            SearchNeigbhor(neigbor.Value, invert, index, -1);
            SearchNeigbhor(neigbor.Value, invert, index, 1);
        }

        LinkPosList.RemoveAt(index);
        _flagRefresh = true;

        _debugRemove.Add(remove);
    }

    public void SearchNeigbhor(UnitPos lfv, int remove, int idx, int delta)
    {
        if (idx < 0 || idx >= LinkPosList.Count)
        {
            return;
        }

        LinkPos link = LinkPosList.ElementAt(idx);
        if (link.Center == lfv)
        {
            link.Remove((LinkPos.Neighbor)remove);
        }
        else
        {
            SearchNeigbhor(lfv, remove, idx + delta, delta);
        }
    }

    public override void Refresh()
    {
        base.Refresh();
        ComponentCollision.Reset();

        // check if object was destroy
        Alive();
    }

    public void OnDestroy()
    {
        CollisionManager.Instance.UnRegister(ComponentCollision, gameObject.tag.GetHashCode());
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public virtual void Alive()
    {
        // check if object need to be destroy
        if (LinkPosList.Count == 0)
        {
            Tool.Builder.Instance.DestroyGameObject(gameObject, false);
        }
    }

#if (UNITY_EDITOR)
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (Tool.DebugWindowAccess.Instance.Serialize.EnableDrawRemovePos)
        {
            Gizmos.color = Color.magenta;
            Vector3 size = new Vector3(CompMeshGenerator.ParamCubeSize.Value, CompMeshGenerator.ParamCubeSize.Value, CompMeshGenerator.ParamCubeSize.Value);
            Vector3 delta = size / 2.0f;
            foreach (LinkPos pos in _debugRemove)
            {
                Vector3 posVec = new Vector3(pos.Center.x + delta.x, pos.Center.y + delta.y, pos.Center.z + delta.z);
                Gizmos.DrawWireCube(posVec, size);
            }
        }
    }
#endif
}
