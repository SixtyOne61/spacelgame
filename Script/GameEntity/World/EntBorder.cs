using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

// TO DO : use componentMeshGenerator and EntBorderFront
public class EntBorder : VisuelEntity
{
    public Tool.SCRMap ParamMap;
    public Tool.SCROneValue ParamNbChunck;

    private List<Vector3> _vertices;
    private List<int> _triangles;

    // Use this for initialization
    override public void Start ()
    {
        base.Start();
        CreateBorder();
	}

	private void CreateBorder()
    {
        // init array
        _vertices = new List<Vector3>();
        _triangles = new List<int>();

        // all front, then back face
        for(int z = 0; z <= ParamNbChunck.Value * ParamMap.Depth; z += (int)ParamNbChunck.Value * ParamMap.Depth)
        {
            for (int x = 0; x < ParamNbChunck.Value; ++x)
            {
                for (int y = 0; y < ParamNbChunck.Value; ++y)
                {
                    Vector3 start = new Vector3(x * ParamMap.Width, y * ParamMap.Height, z);
                    CreateDownBeam(start, new Vector3(ParamMap.Depth, 1, -1));
                    CreateUpBeam(start, new Vector3(ParamMap.Width, ParamMap.Height, -1));
                    CreateLeftBeam(start, new Vector3(1, ParamMap.Height - 1, 0), false);
                    CreateRightBeam(start, new Vector3(ParamMap.Width, ParamMap.Height - 1, -1), true);
                }
            }
        }

        // all left, then right face
        for(int x = 0; x <= ParamNbChunck.Value * ParamMap.Width; x += (int)ParamNbChunck.Value * ParamMap.Width)
        {
            for (int z = 0; z < ParamNbChunck.Value; ++z)
            {
                for (int y = 0; y < ParamNbChunck.Value; ++y)
                {
                    Vector3 start = new Vector3(x, y * ParamMap.Height, z * ParamMap.Depth);
                    CreateDownBeam(start, new Vector3(-1, 1, ParamMap.Depth));
                    CreateUpBeam(start, new Vector3(-1, ParamMap.Height, ParamMap.Depth));
                    CreateLeftBeam(start, new Vector3(-1, ParamMap.Height - 1, ParamMap.Depth), true);
                    CreateRightBeam(start, new Vector3(0, ParamMap.Height - 1, 1), false);
                }
            }
        }

        // all bottom, then top face
        for (int y = 0; y <= ParamNbChunck.Value * ParamMap.Height; y += (int)ParamNbChunck.Value * ParamMap.Height)
        {
            for (int x = 0; x < ParamNbChunck.Value; ++x)
            {
                for (int z = 0; z < ParamNbChunck.Value; ++z)
                {
                    Vector3 start = new Vector3(x * ParamMap.Width, y, z * ParamMap.Depth);
                    CreateBottomBeam(start, new Vector3(ParamMap.Width, 1, 1));
                    CreateTopBeam(start, new Vector3(ParamMap.Width, 1, ParamMap.Depth));
                    CreateRightBeamTop(start, new Vector3(1, -1, ParamMap.Depth - 1));
                    CreateLeftBeamTop(start, new Vector3(ParamMap.Width, -1, ParamMap.Depth - 1));
                }
            }
        }

        _mesh.Clear();
        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = _mesh;
    }

    private void AddTriangles(int deb)
    {
        int[] triangles = {
            deb, deb+2, deb+1, //face front
	        deb, deb+3, deb+2,
            deb+2, deb+3, deb+4, //face top
	        deb+2, deb+4, deb+5,
            deb+1, deb+2, deb+5, //face right
	        deb+1, deb+5, deb+6,
            deb+0, deb+7, deb+4, //face left
	        deb+0, deb+4, deb+3,
            deb+5, deb+4, deb+7, //face back
	        deb+5, deb+7, deb+6,
            deb, deb+6, deb+7, //face bottom
	        deb, deb+1, deb+6
        };

        foreach (int numEdge in triangles)
        {
            _triangles.Add(numEdge);
        }
    }

    #region Create Beam

    private void CreateDownBeam(Vector3 aStart, Vector3 aDelta)
    {
        int deb = _vertices.Count;
        // we want to create down wall
        _vertices.Add(new Vector3(0,            0,              0) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x, 0,              0) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x, 1 * aDelta.y,   0) + aStart);
        _vertices.Add(new Vector3(0,            1 * aDelta.y,   0) + aStart);

        _vertices.Add(new Vector3(0,            1 * aDelta.y,   1 * aDelta.z) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x, 1 * aDelta.y,   1 * aDelta.z) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x, 0,              1 * aDelta.z) + aStart);
        _vertices.Add(new Vector3(0,            0,              1 * aDelta.z) + aStart);

        AddTriangles(deb);
    }

    private void CreateUpBeam(Vector3 aStart, Vector3 aDelta)
    {
        int deb = _vertices.Count;
        // we want to create down wall
        _vertices.Add(new Vector3(0, aDelta.y - 1, 0) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x, 1 * aDelta.y - 1, 0) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x, 1 * aDelta.y, 0) + aStart);
        _vertices.Add(new Vector3(0, 1 * aDelta.y, 0) + aStart);

        _vertices.Add(new Vector3(0, 1 * aDelta.y, 1 * aDelta.z) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x, 1 * aDelta.y, 1 * aDelta.z) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x, 1 * aDelta.y - 1, 1 * aDelta.z) + aStart);
        _vertices.Add(new Vector3(0, 1 * aDelta.y - 1, 1 * aDelta.z) + aStart);

        AddTriangles(deb);
    }

    private void CreateLeftBeam(Vector3 aStart, Vector3 aDelta, bool aIsInverted)
    {
        int deb = _vertices.Count;
        _vertices.Add(new Vector3(0,            1,              aDelta.z + (aIsInverted ? - 1 : + 0)) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x, 1,              aDelta.z + (aIsInverted ? - 1 : + 0)) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x, 1 * aDelta.y,   aDelta.z + (aIsInverted ? - 1 : + 0)) + aStart);
        _vertices.Add(new Vector3(0,            1 * aDelta.y,   aDelta.z + (aIsInverted ? - 1 : + 0)) + aStart);

        _vertices.Add(new Vector3(1 * aDelta.x, 1 * aDelta.y,   aDelta.z + (!aIsInverted ? - 1 : + 0)) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x, 1,              aDelta.z + (!aIsInverted ? - 1 : + 0)) + aStart);
        _vertices.Add(new Vector3(0,            1,              aDelta.z + (!aIsInverted ? - 1 : + 0)) + aStart);
        _vertices.Add(new Vector3(0,            1 * aDelta.y,   aDelta.z + (!aIsInverted ? - 1 : + 0)) + aStart);

        AddTriangles(deb);
    }

    private void CreateRightBeam(Vector3 aStart, Vector3 aDelta, bool aIsInverted)
    {
        int deb = _vertices.Count;
        
        _vertices.Add(new Vector3(1 * aDelta.x - 1, 1,              aIsInverted ? 0 : 1 * aDelta.z) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x,     1,              aIsInverted ? 0 : 1 * aDelta.z) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x,     1 * aDelta.y,   aIsInverted ? 0 : 1 * aDelta.z) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x - 1, 1 * aDelta.y,   aIsInverted ? 0 : 1 * aDelta.z) + aStart);

        _vertices.Add(new Vector3(1 * aDelta.x - 1, 1 * aDelta.y,   !aIsInverted ? 0 : 1 * aDelta.z) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x,     1 * aDelta.y,   !aIsInverted ? 0 : 1 * aDelta.z) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x,     1,              !aIsInverted ? 0 : 1 * aDelta.z) + aStart);
        _vertices.Add(new Vector3(1 * aDelta.x - 1, 1,              !aIsInverted ? 0 : 1 * aDelta.z) + aStart);

        AddTriangles(deb);
    }

    private void CreateBottomBeam(Vector3 aStart, Vector3 aDelta)
    {
        int deb = _vertices.Count;

        _vertices.Add(new Vector3(0,            -1,            -1) + aStart);
        _vertices.Add(new Vector3(0,            -1,             0) + aStart);
        _vertices.Add(new Vector3(0,            0,              0) + aStart);
        _vertices.Add(new Vector3(0,            0,             -1) + aStart);

        _vertices.Add(new Vector3(aDelta.x,     0,             -1) + aStart);
        _vertices.Add(new Vector3(aDelta.x,     0,             0) + aStart);
        _vertices.Add(new Vector3(aDelta.x,     -1,            0) + aStart);
        _vertices.Add(new Vector3(aDelta.x,     -1,            -1) + aStart);

        AddTriangles(deb);
    }

    private void CreateTopBeam(Vector3 aStart, Vector3 aDelta)
    {
        int deb = _vertices.Count;

        _vertices.Add(new Vector3(0,            -1,         aDelta .z) + aStart);
        _vertices.Add(new Vector3(0,            -1,         aDelta.z + 1) + aStart);
        _vertices.Add(new Vector3(0,            0,          aDelta.z + 1) + aStart);
        _vertices.Add(new Vector3(0,            0,          aDelta.z) + aStart);

        _vertices.Add(new Vector3(aDelta.x,     0,          aDelta.z) + aStart);
        _vertices.Add(new Vector3(aDelta.x,     0,          aDelta.z + 1) + aStart);
        _vertices.Add(new Vector3(aDelta.x,     -1,         aDelta.z + 1) + aStart);
        _vertices.Add(new Vector3(aDelta.x,     -1,         aDelta.z) + aStart);

        AddTriangles(deb);
    }

    private void CreateRightBeamTop(Vector3 aStart, Vector3 aDelta)
    {
        int deb = _vertices.Count;

        _vertices.Add(new Vector3(0,            1 * aDelta.y,   0) + aStart);
        _vertices.Add(new Vector3(0,            1 * aDelta.y,   aDelta.z) + aStart);
        _vertices.Add(new Vector3(0,            0,              aDelta.z) + aStart);
        _vertices.Add(new Vector3(0,            0,              0) + aStart);

        _vertices.Add(new Vector3(1,            0,              0) + aStart);
        _vertices.Add(new Vector3(1,            0,              aDelta.z) + aStart);
        _vertices.Add(new Vector3(1,            1 * aDelta.y,   aDelta.z) + aStart);
        _vertices.Add(new Vector3(1,            1 * aDelta.y,   0) + aStart);

        AddTriangles(deb);
    }

    private void CreateLeftBeamTop(Vector3 aStart, Vector3 aDelta)
    {
        int deb = _vertices.Count;

        _vertices.Add(new Vector3(aDelta.x - 1, 1 * aDelta.y, 0) + aStart);
        _vertices.Add(new Vector3(aDelta.x - 1, 1 * aDelta.y, aDelta.z) + aStart);
        _vertices.Add(new Vector3(aDelta.x - 1, 0, aDelta.z) + aStart);
        _vertices.Add(new Vector3(aDelta.x - 1, 0, 0) + aStart);

        _vertices.Add(new Vector3(aDelta.x, 0, 0) + aStart);
        _vertices.Add(new Vector3(aDelta.x, 0, aDelta.z) + aStart);
        _vertices.Add(new Vector3(aDelta.x, 1 * aDelta.y, aDelta.z) + aStart);
        _vertices.Add(new Vector3(aDelta.x, 1 * aDelta.y, 0) + aStart);

        AddTriangles(deb);
    }

    #endregion
}
