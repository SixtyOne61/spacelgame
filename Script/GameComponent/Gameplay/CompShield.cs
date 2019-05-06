using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

[System.Serializable]
public class CompShield : ComponentBase
{
    private static readonly Vector4[] defaultEmptyVector = new Vector4[] { new Vector4(0, 0, 0, 0) };

    // list of current impact
    private List<Vector4> _impacts = new List<Vector4>();
    // impact duration use for make the wave
    private float _impactDuration;
    // damage taken
    private float _dmgTaken;

    [Tooltip("Material of this shield")]
    public Material ShieldMaterial;
    [Tooltip("Param for shield")]
    public Tool.SCRShield ParamShield;
    [HideInInspector]
    // size of the shield (sphere size)
    public float ShieldSize = 1.0f;

    public override void Start()
    {
        base.Start();

        _impactDuration = ParamShield.Duration;
        _dmgTaken = ParamShield.DmgTaken;
        CollisionManager.Instance.Register(this);
    }

    public override void Update()
	{
        UpdateMaterial();
        UpdateImapctLife();

		base.Update();
	}

    private void UpdateMaterial()
    {
        ShieldMaterial.SetInt("_PointsSize", _impacts.Count);
        if (_impacts.Count <= 0)
        {
            ShieldMaterial.SetVectorArray("_Points", defaultEmptyVector);
        }
        else
        {
            ShieldMaterial.SetVectorArray("_Points", _impacts.ToArray());
        }
    }

    private void UpdateImapctLife()
    {
        for(int i = 0; i < _impacts.Count;)
        {
            Vector4 impact = _impacts[i];
            impact.w += Time.deltaTime / _impactDuration;
            if(impact.w >= _impactDuration)
            {
                _impacts.RemoveAt(i);
            }
            else
            {
                _impacts[i] = impact;
                ++i;
            }
        }
    }

    public void Hit(CompCollisionBullet compBullet)
    {
        foreach(LinkPos pos in compBullet.LinkPosList)
        {
            Vector3 worldPos = compBullet.Owner.transform.TransformPoint(pos.Center.ToVec3());
            if(Vector3.Distance(worldPos, Owner.transform.position) < ShieldSize)
            {
                AddImpact(Owner.transform.InverseTransformPoint(worldPos));
                // destroy bullet
                Builder.Instance.DestroyGameObject(compBullet.Owner);
                // update dmg taken
                _dmgTaken -= compBullet.Owner.GetComponent<VolumeEntity>().ParamAttribut.Damage;
                
                // destroy shield
                if(_dmgTaken <= 0)
                {
                	Builder.Instance.DestroyGameObject(Owner);
                }
            }
        }
    }

    public void AddImpact(Vector3 point)
    {
        _impacts.Add(point);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        CollisionManager.Instance.UnRegister(this);
    }
}
