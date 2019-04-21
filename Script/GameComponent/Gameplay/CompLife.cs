using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

[System.Serializable]
public class CompLife : ComponentBase {

    // contain all param for life object
    public Tool.SCROneValue Param;

    // timer before end
    [HideInInspector]
    public float Timer;

    // Use this for initialization
    override public void Start ()
    {
        Reset();
    }

    // Update is called once per frame
    override public void Update ()
    {
        Timer += Time.deltaTime;
        if (Timer >= Param.Value)
        {
            Tool.Builder.Instance.DestroyGameObject(Owner, false);
        }
    }

    public void Reset()
    {
        Timer = 0.0f;
    }

    public float GetRatio()
    {
        return Timer / Param.Value;
    }
}
