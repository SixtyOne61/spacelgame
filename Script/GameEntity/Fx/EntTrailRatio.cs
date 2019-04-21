using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntTrailRatio : EntTrail
{
    // ratio for time
    [HideInInspector]
    public float Ratio;
    private float _timeBase;

    public override void Start()
    {
        base.Start();

        // base time
        _timeBase = _trail.time;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        _trail.time = _timeBase * Ratio;
    }
}
