﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public class EntRock : EntEnvironment
{
	// To do remove 
    public override void Start()
    {
        base.Start();
        if(ParamAttribut != null)
        {
            foreach (LinkPos pos in LinkPosList)
            {
                pos.Life = ParamAttribut.Life;
            }
        }
    }
}
