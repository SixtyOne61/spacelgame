using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public class EntBullet : CollideEntity
{
    // components
    public CompLife ComponentLife;

    // Param
    public Tool.SCRValueCurve ParamSpeed;

	// Use this for initialization
	override public void Start ()
    {
        LinkPosList.Add(new LinkPos(new UnitPos(0, 0, 0), ParamAttribut != null ? ParamAttribut.Life : int.MaxValue));

        AddComponent(ComponentLife);
        base.Start();

        MyMeshCollider.convex = true;
	}
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();

        // move
        float ratioTimer = ComponentLife.GetRatio();
        float evaluteSpeed = ParamSpeed.Value * ParamSpeed.Curve.Evaluate(ratioTimer);
        transform.position += transform.forward * evaluteSpeed * Time.deltaTime;
    }

    public override void Hit(ContactPoint[] _points, int _dmg)
    {
        // will destroy object
        LinkPosList.Clear();
        _flagRefresh = true;
        // spawn spark

        GameObject obj = Tool.Builder.Instance.Build(Tool.Builder.FactoryType.Fx, (int)Tool.BuilderFx.Type.Sparkle, transform.position, Quaternion.identity, null);
        //obj.GetComponent<EntSparkle>().ImpactNormal = _points[0].normal;
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
