using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public class EntTestMove : CollideEntity
{
    public CompLife ComponentLife;

    public override void Start()
    {
        LinkPosList = new List<LinkPos>();
        LinkPosList.Add(new LinkPos(new UnitPos(0, 0, 0), ParamAttribut != null ? ParamAttribut.Life : int.MaxValue));

        AddComponent(ComponentLife);
        base.Start();
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
        transform.position += Vector3.forward * Time.deltaTime;
	}
}
