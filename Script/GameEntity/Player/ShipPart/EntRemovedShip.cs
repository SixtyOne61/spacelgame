using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntRemovedShip : Engine.VolumeEntity
{
    private int _countPos = 0;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if(_countPos != LinkPosList.Count)
        {
            _flagRefresh = true;
            _countPos = LinkPosList.Count;
        }
    }
}
