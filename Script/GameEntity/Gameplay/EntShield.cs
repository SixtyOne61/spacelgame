using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

//[ExecuteInEditMode]
public class EntShield : VisuelEntity
{
    public CompShield ComponentShield;

    public override void Start()
    {
        AddComponent(ComponentShield);
        base.Start();
    }
}
