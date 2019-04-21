﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public class EntBullet : CollideEntity
{
    // components
    public CompLife ComponentLife;
    private CompTrails<EntTrailbullet> _componentTrails;

    // Param
    public Tool.SCRValueCurve ParamSpeed;

	// Use this for initialization
	override public void Start ()
    {
        LinkPosList.Add(new LinkPos(new UnitPos(0, 0, 0), ParamAttribut != null ? ParamAttribut.Life : int.MaxValue));

        // cube size, was on compMeshGenerator
        float halfCubeSize = CompMeshGenerator.ParamCubeSize.Value / 2.0f;
        // add component trails, it will spawn trails needed on start
        _componentTrails = new CompTrails<EntTrailbullet>(new Vector3(halfCubeSize, halfCubeSize, 0.0f));

        AddComponent(ComponentLife);
        AddComponent(_componentTrails);
        base.Start();
	}
	
	// Update is called once per frame
	override public void Update ()
    {
        base.Update();

        // move
        float ratioTimer = ComponentLife.GetRatio();
        float evaluteSpeed = ParamSpeed.Value * ParamSpeed.Curve.Evaluate(ratioTimer);
        transform.position += transform.forward * evaluteSpeed * Time.deltaTime;
    }

#if (UNITY_EDITOR)
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (Tool.DebugWindowAccess.Instance.Serialize.EnableDrawBulletCollision)
        {
            Gizmos.color = Color.yellow;
            foreach (LinkPos pos in LinkPosList)
            {
                Vector3 posVec = new Vector3(pos.Center.x, pos.Center.y, pos.Center.z);
                Gizmos.DrawWireSphere(transform.TransformPoint(posVec), CompMeshGenerator.ParamCubeSize.Value);
            }
        }
    }
#endif
}
