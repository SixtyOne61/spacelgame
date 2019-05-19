using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using Tool;

//[ExecuteInEditMode]
public class EntShield : VisuelEntity
{
    public CompShield ComponentShield;
    // max and min in each axis
    [HideInInspector]
    public Vector2Int X;
    [HideInInspector]
    public Vector2Int Y;
    [HideInInspector]
    public Vector2Int Z;

    // for build shield
    private List<Vector3> _vertices = new List<Vector3>();
    private List<int> _triangles = new List<int>();

    public override void Start()
    {
        // TO DO : create mesh with the coord without volume entity
        //CompMeshGenerator.ParamCubeSize = ScriptableObject.CreateInstance(typeof(Tool.SCROneValue)) as Tool.SCROneValue;
        //CompMeshGenerator.ParamCubeSize.Value = 1.0f;
        // compute vertex
        InitVertex();
        AddComponent(ComponentShield);
        base.Start();
    }
    
    public void InitVertex()
    {
    	//LinkPosList.Add(new LinkPos(new UnitPos(X.x, Y.x, Z.x)));
    	//LinkPosList.Add(new LinkPos(new UnitPos(X.y, Y.x, Z.x)));
    	//LinkPosList.Add(new LinkPos(new UnitPos(X.y, Y.y, Z.x)));
    	//LinkPosList.Add(new LinkPos(new UnitPos(X.x, Y.y, Z.x)));
    	//
    	//LinkPosList.Add(new LinkPos(new UnitPos(X.x, Y.x, Z.y)));
    	//LinkPosList.Add(new LinkPos(new UnitPos(X.y, Y.x, Z.y)));
    	//LinkPosList.Add(new LinkPos(new UnitPos(X.y, Y.y, Z.y)));
    	//LinkPosList.Add(new LinkPos(new UnitPos(X.x, Y.y, Z.y)));	
    }
}
