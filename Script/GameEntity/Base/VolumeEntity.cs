using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public class VolumeEntity : VisuelEntity
{
    // Component
    public CompMeshGenerator CompMeshGenerator;
    public CompMaterial CompMaterial;

    [HideInInspector]
    public List<LinkPos> LinkPosList = new List<LinkPos>();

    // set true for refresh on next frame
    protected bool _flagRefresh = false;

    public override void Start()
    {
        CompMeshGenerator.LinkPosList = LinkPosList;

        AddComponent(CompMeshGenerator);
        AddComponent(CompMaterial);
        base.Start();

        GetComponent<MeshFilter>().mesh = CompMeshGenerator.CustomMesh;
    }

    public virtual void Refresh()
    {
        CompMeshGenerator.GenerateMesh();
    }

    public override void Update()
    {
        base.Update();

        if (_flagRefresh)
        {
            Refresh();
            _flagRefresh = false;
        }
    }
}
