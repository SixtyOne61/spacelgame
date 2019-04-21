using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public class EntTest : CollideEntity
{
    public override void Start()
    {
        LinkPosList = new List<LinkPos>();

        for(int x = 0; x < 3; ++x)
        {
            for (int y = 0; y < 4; ++y)
            {
                for (int z = 0; z < 3; ++z)
                {
                    if (x == 0 && y == 1 && z == 0)
                        continue;
                    LinkPosList.Add(new LinkPos(new UnitPos(x, y, z), ParamAttribut != null ? ParamAttribut.Life : int.MaxValue));
                }
            }
        }

        base.Start();
    }
}
