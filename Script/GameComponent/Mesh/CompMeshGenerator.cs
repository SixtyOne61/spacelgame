using System.Collections.Generic;
using UnityEngine;
using Engine;

[System.Serializable]
public class CompMeshGenerator : ComponentMeshBase
{
    public Tool.SCROneValue ParamCubeSize;

    [HideInInspector]
    public List<LinkPos> LinkPosList = new List<LinkPos>();

    private List<Vector3> _vertices = new List<Vector3>();
    private List<int> _triangles = new List<int>();

    public override void Start()
    {
        base.Start();
        GenerateMesh();
    }

    public void GenerateMesh()
    {
        _vertices.Clear();
        _triangles.Clear();

        // we need to determine the outline of the form
        float size = ParamCubeSize.Value;
        foreach (LinkPos curr in LinkPosList)
        {
            Face mask = curr.Mask;
            UnitPos currPos = curr.Center;
            // read mask from linkPos
            if ((mask & Face.Top) == Face.None)
            {
                int deb = _vertices.Count;
                _vertices.Add(new Vector3(currPos.x, currPos.y + size, currPos.z));
                _vertices.Add(new Vector3(currPos.x + size, currPos.y + size, currPos.z));
                _vertices.Add(new Vector3(currPos.x + size, currPos.y + size, currPos.z + size));
                _vertices.Add(new Vector3(currPos.x, currPos.y + size, currPos.z + size));
                AddTriangles(deb);
            }
            if ((mask & Face.Bot) == Face.None)
            {
                int deb = _vertices.Count;
                _vertices.Add(new Vector3(currPos.x + size, currPos.y, currPos.z + size));
                _vertices.Add(new Vector3(currPos.x + size, currPos.y, currPos.z));
                _vertices.Add(new Vector3(currPos.x, currPos.y, currPos.z));
                _vertices.Add(new Vector3(currPos.x, currPos.y, currPos.z + size));
                AddTriangles(deb);
            }
            if ((mask & Face.Right) == Face.None)
            {
                int deb = _vertices.Count;
                _vertices.Add(new Vector3(currPos.x + size, currPos.y, currPos.z + size));
                _vertices.Add(new Vector3(currPos.x + size, currPos.y + size, currPos.z + size));
                _vertices.Add(new Vector3(currPos.x + size, currPos.y + size, currPos.z));
                _vertices.Add(new Vector3(currPos.x + size, currPos.y, currPos.z));
                AddTriangles(deb);
            }
            if ((mask & Face.Left) == Face.None)
            {
                int deb = _vertices.Count;
                _vertices.Add(new Vector3(currPos.x, currPos.y, currPos.z + size));
                _vertices.Add(new Vector3(currPos.x, currPos.y, currPos.z));
                _vertices.Add(new Vector3(currPos.x, currPos.y + size, currPos.z));
                _vertices.Add(new Vector3(currPos.x, currPos.y + size, currPos.z + size));
                AddTriangles(deb);
            }
            if ((mask & Face.Front) == Face.None)
            {
                int deb = _vertices.Count;
                _vertices.Add(new Vector3(currPos.x, currPos.y + size, currPos.z));
                _vertices.Add(new Vector3(currPos.x, currPos.y, currPos.z));
                _vertices.Add(new Vector3(currPos.x + size, currPos.y, currPos.z));
                _vertices.Add(new Vector3(currPos.x + size, currPos.y + size, currPos.z));
                AddTriangles(deb);
            }
            if ((mask & Face.Back) == Face.None)
            {
                int deb = _vertices.Count;
                _vertices.Add(new Vector3(currPos.x + size, currPos.y, currPos.z + size));
                _vertices.Add(new Vector3(currPos.x, currPos.y, currPos.z + size));
                _vertices.Add(new Vector3(currPos.x, currPos.y + size, currPos.z + size));
                _vertices.Add(new Vector3(currPos.x + size, currPos.y + size, currPos.z + size));
                AddTriangles(deb);
            }
        }

        CustomMesh.Clear();
        CustomMesh.vertices = _vertices.ToArray();
        CustomMesh.triangles = _triangles.ToArray();
        CustomMesh.RecalculateNormals();
    }

    private void AddTriangles(int deb)
    {
        int[] triangles = {
            deb, deb+2, deb+1,
	        deb, deb+3, deb+2
        };

        foreach (int numEdge in triangles)
        {
            _triangles.Add(numEdge);
        }
    }
}
