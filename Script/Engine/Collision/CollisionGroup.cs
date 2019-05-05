using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionGroup<T>
{
    // list of all collision on this group
    public List<T> Components = new List<T>();

    public CollisionGroup()
    {

    }

    public void Add(T comp)
    {
        Components.Add(comp);
    }

    public bool Remove(T comp)
    {
        return Components.Remove(comp);
    }
}
