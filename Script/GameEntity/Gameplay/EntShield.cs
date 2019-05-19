using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using Tool;

//[ExecuteInEditMode]
public class EntShield : VolumeEntity
{
    public CompShield ComponentShield;
    // max and min in each axis
    [HideInInspector]
    public Vector2Int X;
    [HideInInspector]
    public Vector2Int Y;
    [HideInInspector]
    public Vector2Int Z;

    public override void Start()
    {
    	// compute vertex
    	InitVertex();
        AddComponent(ComponentShield);
        base.Start();
    }
    
    public void InitVertex()
    {
    	LinkPosList.Add(new LinkPos(new UnitPos(X.x, Y.x, Z.x)));
    	LinkPosList.Add(new LinkPos(new UnitPos(X.y, Y.x, Z.x)));
    	LinkPosList.Add(new LinkPos(new UnitPos(X.y, Y.y, Z.x)));
    	LinkPosList.Add(new LinkPos(new UnitPos(X.x, Y.y, Z.x)));
    	
    	LinkPosList.Add(new LinkPos(new UnitPos(X.x, Y.x, Z.y)));
    	LinkPosList.Add(new LinkPos(new UnitPos(X.y, Y.x, Z.y)));
    	LinkPosList.Add(new LinkPos(new UnitPos(X.y, Y.y, Z.y)));
    	LinkPosList.Add(new LinkPos(new UnitPos(X.x, Y.y, Z.y)));	
    }
}
