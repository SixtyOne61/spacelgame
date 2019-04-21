using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

[RequireComponent(typeof(TrailRenderer))]
public class EntTrail : SpacelEntity
{
    [Tooltip("param for trail, need size cube")]
    public Tool.SCROneValue ParamSizeCube;

    // trail component
    protected TrailRenderer _trail = null;
    
    public override void Start()
    {
        base.Start();

        // init trail
        _trail = GetComponent<TrailRenderer>();
        float cubeSize = ParamSizeCube.Value;

        // set multiplier width
        _trail.widthMultiplier = cubeSize;
    }
}
