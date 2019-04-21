using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompRatioTrails : CompTrails<EntTrailRatio>
{
    // base for compute ratio
    [HideInInspector]
    public float Base;

    // value who evolve
    [HideInInspector]
    public float Evolve;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        foreach (EntTrailRatio entityTrail in _trails)
        {
            entityTrail.Ratio = Evolve / Base;
        }
    }
}
