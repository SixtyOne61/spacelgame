using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using System.Linq;

public class CollideEntity<T> : VolumeEntity where T : CompCollision, new()
{
    // component collision needed
    protected T _componentCollision;

    public override void Start()
    {
        // init component collision
        _componentCollision = new T();
        _componentCollision.ParamCubeSize = CompMeshGenerator.ParamCubeSize;
        _componentCollision.LinkPosList = LinkPosList;

        AddComponent(_componentCollision);
        base.Start();
    }

    public override void Refresh()
    {
        base.Refresh();
        _componentCollision.Reset();

        // check if object was destroy
        Alive();
    }

    public void OnDestroy()
    {
        _componentCollision.OnDestroy();
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
                // add in world space
                posVec += transform.position;
                Gizmos.DrawWireCube(posVec, size);
            }
        }
    }
#endif
}
