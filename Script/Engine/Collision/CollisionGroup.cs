using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionGroup
{
    // group id
    public int Id;
    // list of all collision on this group
    public List<CompCollision> Components = new List<CompCollision>();

    public CollisionGroup()
    {

    }

    public CollisionGroup(int id)
    {
        Id = id;
    }

    public void Add(CompCollision comp)
    {
        Components.Add(comp);
    }

    public bool Remove(CompCollision comp)
    {
        return Components.Remove(comp);
    }
}
