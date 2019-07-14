using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using Tool;

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
    [HideInInspector]
    // center of shield
    public Vector3 Center = Vector3.zero;

    public override void Start()
    {
        base.Start();

        _impactDuration = ParamShield.Duration;
        _dmgTaken = ParamShield.DamageTaken;

        // TO DO 
        //CollisionManager.Instance.Register(this);
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

    public void Hit(ComponentCollision comp)
    {
        // TO DO : maybe change this
        foreach(LinkPos pos in comp.LinkPosList)
        {
            Vector3 worldPos = comp.Owner.transform.TransformPoint(pos.Center.ToVec3());
            if(Vector3.Distance(worldPos, Owner.transform.position) <= ShieldSize)
            {
            	PerfectHit(worldPos);
                AddImpact(Owner.transform.InverseTransformPoint(worldPos));
                // update dmg taken
                _dmgTaken -= comp.Owner.GetComponent<VolumeEntity>().ParamAttribut.Damage;
                // destroy bullet
                Builder.Instance.DestroyGameObject(comp.Owner, false);
                
                // destroy shield
                if(_dmgTaken <= 0)
                {
                	Builder.Instance.DestroyGameObject(Owner, false);
                }
            }
        }
    }
    
    public void PerfectHit(Vector3 point)
    {
    	// create many aabb for describe this object
    }

    public void AddImpact(Vector3 point)
    {
        _impacts.Add(point);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        // TO DO
        //CollisionManager.Instance.UnRegister(this);
    }
}
