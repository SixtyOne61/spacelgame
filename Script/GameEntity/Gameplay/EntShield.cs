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
        // compute vertex
        GenerateCubeMesh();
        AddComponent(ComponentShield);
        base.Start();
    }
    
    private void GenerateCubeMesh()
    {
    	_vertices.Add(new Vector3(X.x, Y.x, Z.x));
    	_vertices.Add(new Vector3(X.y, Y.x, Z.x));
    	_vertices.Add(new Vector3(X.y, Y.y, Z.x));
    	_vertices.Add(new Vector3(X.x, Y.y, Z.x));
    	
    	_vertices.Add(new Vector3(X.x, Y.y, Z.y));
    	_vertices.Add(new Vector3(X.y, Y.y, Z.y));
    	_vertices.Add(new Vector3(X.y, Y.x, Z.y));
    	_vertices.Add(new Vector3(X.x, Y.x, Z.y));

        // triangle
        _triangles = new List<int>(new int[] { 0, 2, 1, //face front
    		0, 3, 2,
            2, 3, 4, //face top
    		2, 4, 5,
            1, 2, 5, //face right
    		1, 5, 6,
            0, 7, 4, //face left
    		0, 4, 3,
            5, 4, 7, //face back
    		5, 7, 6,
            0, 6, 7, //face bottom
    		0, 1, 6});
    	
    	Mesh mesh = GetComponent<MeshFilter>().mesh;
    	mesh.Clear();
    	mesh.vertices = _vertices.ToArray();
    	mesh.triangles = _triangles.ToArray();
    	;
    	mesh.RecalculateNormals ();
    }
}
