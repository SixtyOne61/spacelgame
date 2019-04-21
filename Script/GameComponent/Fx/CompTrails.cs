using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public class CompTrails<T> : ComponentBase
{
    // TO DO : link speed ratio to Time of trail, we define the 1.0f in prefab

    // list of local position of each trails
    protected List<Vector3> _localPositions = new List<Vector3>();

    // list of trails
    protected List<T> _trails = new List<T>();

    protected CompTrails() 
        : base()
    {
    }

    public CompTrails(Vector3 localPosition)
        : base()
    {
        _localPositions.Add(localPosition);
    }

    public CompTrails(List<Vector3> localPositions)
        : base()
    {
        _localPositions = localPositions;
    }

    public override void Start()
    {
        base.Start();

        foreach (Vector3 localPosition in _localPositions)
        {
            AddTrails(localPosition);
        }
    }

    public void AddTrails(Vector3 localPosition)
    {
        // spawn trail, save entity
        _trails.Add(Tool.Builder.Instance.Build(Tool.Builder.FactoryType.Fx, (int)Tool.BuilderFx.Type.TrailBullet, Owner.transform.position + localPosition, Quaternion.identity, Owner.transform).GetComponent<T>());
    }
}
