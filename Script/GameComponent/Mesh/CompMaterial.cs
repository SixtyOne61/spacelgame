using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

[System.Serializable]
public class CompMaterial : ComponentBase
{
    public Tool.SCRMaterial ParamMaterial;
    // Use this for initialization
    public override void Start()
    {
        Owner.GetComponent<MeshRenderer>().material = ParamMaterial.Material;
        base.Start();
    }
}
