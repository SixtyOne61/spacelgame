using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using System.Linq;

public class CollideEntity : VolumeEntity
{
    // component collision needed
    public CompCollision ComponentCollision;

    public override void Start()
    {
        // init component collision
        ComponentCollision = new CompCollision();
        ComponentCollision.ParamCubeSize = CompMeshGenerator.ParamCubeSize;
        ComponentCollision.LinkPosList = LinkPosList;

        AddComponent(ComponentCollision);
        base.Start();
    }

    public override void Refresh()
    {
        base.Refresh();
        ComponentCollision.Reset();

        // check if object was destroy
        Alive();
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
