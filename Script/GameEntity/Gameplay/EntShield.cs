using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using Tool;

public class EntShield : VisuelEntity
{
    public CompShield ComponentShield;
    // max and min in each axis
    [HideInInspector]
    public Vector2 X;
    [HideInInspector]
    public Vector2 Y;
    [HideInInspector]
    public Vector2 Z;
    [HideInInspector]
    // radius of the shield, depend of x,y,z
    public float Radius = 0.0f;
    [HideInInspector]
	// for build shield
    public Vector3 Center = Vector3.zero;

    public override void Start()
    {
        // center 
        Center = new Vector3((X.y + X.x) / 2.0f, (Y.y + Y.x) / 2.0f, (Z.y + Z.x) / 2.0f);
        // compute vertex for create icosphere
        Create();
        // move object to center
        transform.position = Center;
        // init component
        ComponentShield.ShieldSize = Radius;
        ComponentShiele.Center = Center;
        AddComponent(ComponentShield);
        base.Start();
    }
    
    private struct TriangleIndices
    {
        public int v1;
        public int v2;
        public int v3;

        public TriangleIndices(int v1, int v2, int v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
    }

    // return index of point in the middle of p1 and p2
    private int getMiddlePoint(int p1, int p2, ref List<Vector3> vertices, ref Dictionary<long, int> cache, float radius)
    {
        // first check if we have it already
        bool firstIsSmaller = p1 < p2;
        long smallerIndex = firstIsSmaller ? p1 : p2;
        long greaterIndex = firstIsSmaller ? p2 : p1;
        long key = (smallerIndex << 32) + greaterIndex;

        int ret;
        if (cache.TryGetValue(key, out ret))
        {
            return ret;
        }

        // not in cache, calculate it
        Vector3 point1 = vertices[p1];
        Vector3 point2 = vertices[p2];
        Vector3 middle = new Vector3
        (
            (point1.x + point2.x) / 2f,
            (point1.y + point2.y) / 2f,
            (point1.z + point2.z) / 2f
        );

        // add vertex makes sure point is on unit sphere
        int i = vertices.Count;
        vertices.Add(middle.normalized * radius);

        // store it, return index
        cache.Add(key, i);

        return i;
    }

    public void Create()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        List<Vector3> vertList = new List<Vector3>();

        Radius = Mathf.Max(X.y, Mathf.Max(Y.y, Z.y));

        // create 12 vertices of a icosahedron
        float t = (1f + Mathf.Sqrt(5f)) / 2f;

        vertList.Add(new Vector3(-1f, t, 0f).normalized * Radius);
        vertList.Add(new Vector3(1f, t, 0f).normalized * Radius);
        vertList.Add(new Vector3(-1f, -t, 0f).normalized * Radius);
        vertList.Add(new Vector3(1f, -t, 0f).normalized * Radius);

        vertList.Add(new Vector3(0f, -1f, t).normalized * Radius);
        vertList.Add(new Vector3(0f, 1f, t).normalized * Radius);
        vertList.Add(new Vector3(0f, -1f, -t).normalized * Radius);
        vertList.Add(new Vector3(0f, 1f, -t).normalized * Radius);

        vertList.Add(new Vector3(t, 0f, -1f).normalized * Radius);
        vertList.Add(new Vector3(t, 0f, 1f).normalized * Radius);
        vertList.Add(new Vector3(-t, 0f, -1f).normalized * Radius);
        vertList.Add(new Vector3(-t, 0f, 1f).normalized * Radius);


        // create 20 triangles of the icosahedron
        List<TriangleIndices> faces = new List<TriangleIndices>();

        // 5 faces around point 0
        faces.Add(new TriangleIndices(0, 11, 5));
        faces.Add(new TriangleIndices(0, 5, 1));
        faces.Add(new TriangleIndices(0, 1, 7));
        faces.Add(new TriangleIndices(0, 7, 10));
        faces.Add(new TriangleIndices(0, 10, 11));

        // 5 adjacent faces 
        faces.Add(new TriangleIndices(1, 5, 9));
        faces.Add(new TriangleIndices(5, 11, 4));
        faces.Add(new TriangleIndices(11, 10, 2));
        faces.Add(new TriangleIndices(10, 7, 6));
        faces.Add(new TriangleIndices(7, 1, 8));

        // 5 faces around point 3
        faces.Add(new TriangleIndices(3, 9, 4));
        faces.Add(new TriangleIndices(3, 4, 2));
        faces.Add(new TriangleIndices(3, 2, 6));
        faces.Add(new TriangleIndices(3, 6, 8));
        faces.Add(new TriangleIndices(3, 8, 9));

        // 5 adjacent faces 
        faces.Add(new TriangleIndices(4, 9, 5));
        faces.Add(new TriangleIndices(2, 4, 11));
        faces.Add(new TriangleIndices(6, 2, 10));
        faces.Add(new TriangleIndices(8, 6, 7));
        faces.Add(new TriangleIndices(9, 8, 1));

        mesh.vertices = vertList.ToArray();

        List<int> triList = new List<int>();
        for (int i = 0; i < faces.Count; i++)
        {
            triList.Add(faces[i].v1);
            triList.Add(faces[i].v2);
            triList.Add(faces[i].v3);
        }
        mesh.triangles = triList.ToArray();
        mesh.uv = new Vector2[mesh.vertices.Length];

        Vector3[] normales = new Vector3[vertList.Count];
        for (int i = 0; i < normales.Length; i++)
            normales[i] = vertList[i].normalized;


        mesh.normals = normales;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
