using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public class HelperGenerateWorld
{
    // contain all unitary object, each sub list was one object
    [HideInInspector]
    public List<List<LinkPos>> SubList = new List<List<LinkPos>>();

    private List<UnitPos> _globalList = new List<UnitPos>();
    // treshold min and max for noise
    private Vector2 _thresholds;
    public Vector2 Thresholds
    {
        get { return _thresholds; }
    }
    // origine of object
    private Vector3 _origine;
    public Vector3 Origine
    {
        get { return _origine; }
    }
    // size of cube
    private Tool.SCROneValue _paramSize = null;
    public Tool.SCROneValue ParamSize
    {
        get { return _paramSize; }
    }

    public HelperGenerateWorld(Vector2 clamp, Tool.SCROneValue paramSize, Vector3 origine)
    {
        _thresholds = clamp;
        _origine = origine;
        _paramSize = paramSize;
    }

    public void Add(UnitPos pos)
    {
        float noise = Math.PerlinNoise3D(pos.x + _origine.x, pos.y + _origine.y, pos.z + _origine.z, 0.020f, 4);
        if(_thresholds.x <= noise && _thresholds.y >= noise)
        {
            _globalList.Add(pos);
        }
    }

    public void DispachPosInUnitaryObject()
    {
        // first we need to sort our list for optimize, it's order by distance to 0
        _globalList.Sort();

        while (_globalList.Count != 0)
        {
            SubList.Add(new List<LinkPos>());
            // first elem
            LinkPos first = new LinkPos(_globalList.ElementAt(0));
            AddElemInLastSubList(first);
            // recursivly, add neighbors, and neigbhor add their neigbhor
            AddNeighbors(first);
        }
    }

    private void AddElemInLastSubList(LinkPos pos)
    {
        SubList.Last().Add(pos);
    }

    private bool HasElemInLastSubList(LinkPos pos)
    {
        return SubList.Last().Contains(pos);
    }

    private void SetElemInLastSubList(ref LinkPos pos)
    {
        if(HasElemInLastSubList(pos))
        {
            foreach(LinkPos subPos in SubList.Last())
            {
                if(pos.Center == subPos.Center)
                {
                    pos = subPos;
                    break;
                }
            }
        }
    }

    private void AddNeighbors(LinkPos pos)
    {
        // Top
        AddNeighbor(pos, 0, _paramSize.Value, 0, LinkPos.Neighbor.Top);
        // Bottom
        AddNeighbor(pos, 0, -_paramSize.Value, 0, LinkPos.Neighbor.Bottom);
        // left
        AddNeighbor(pos, -_paramSize.Value, 0, 0, LinkPos.Neighbor.Left);
        // right
        AddNeighbor(pos, _paramSize.Value, 0, 0, LinkPos.Neighbor.Right);
        // front
        AddNeighbor(pos, 0, 0, -_paramSize.Value, LinkPos.Neighbor.Front);
        // back
        AddNeighbor(pos, 0, 0, _paramSize.Value, LinkPos.Neighbor.Back);

        // all neighbor was add, we can remove this pos
        _globalList.Remove(pos.Center);
    }

    private void AddNeighbor(LinkPos pos, float x, float y, float z, LinkPos.Neighbor neighbor)
    {
        // already has neighbor
        if(pos.Has(neighbor))
        {
            return;
        }

        LinkPos newLinkPos = new LinkPos(pos.Center);
        newLinkPos.Center.x += x;
        newLinkPos.Center.y += y;
        newLinkPos.Center.z += z;

        // if this coord exist
        if(_globalList.Contains(newLinkPos.Center))
        {
            // add link
            pos.Add(neighbor, newLinkPos.Center);

            // TO DO : not optim, may be change container
            // check if we have already this element
            SetElemInLastSubList(ref newLinkPos);

            int invert = (int)neighbor * -1;
            newLinkPos.Add((LinkPos.Neighbor)invert, pos.Center);

            if (!HasElemInLastSubList(newLinkPos))
            {
                // add in sub list
                AddElemInLastSubList(newLinkPos);
                // add next neighbors
                AddNeighbors(newLinkPos);
            }
        }
    }
}
