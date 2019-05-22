using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Engine;

// to do remove this file
[System.Serializable]
public class CompSimpleCollision : ComponentBase
{
	[HideInInspector]
    public Vector2Int X;
    [HideInInspector]
    public Vector2Int Y;
    [HideInInspector]
    public Vector2Int Z;
    
    // use for define box
    private BoxParam _boxParam = new BoxParam();

	// center
	private Vector3 _center;
	// largest distance
	private float _largestDist;
	
    public override void Start()
    {
        base.Start();
    }
    
    public void Reset()
    {
    	_center = new Vector3(X.y - X.x, Y.y - Y.x, Z.y - Z.x);
    	Vector3 corner = new Vector3(X.y, Y.y, Z.y);
    	_largestDist = Vector3.Distance(_center, corner);
    	
    	_boxParam = new BoxParam(_center);
        _boxParam.x.Clamp = X;
        _boxParam.y.Clamp = Y;
        _boxParam.z.Clamp = Z);

    }

    public void Hit(CompCollision other)
    {
        if(HitShpereBox(other))
        {
            if(HitBoundBox(other))
            {
                DestroyCollider(other);
            }
        }
    }

    private bool HitShpereBox(CompCollision other)
    {
        Vector3 otherPos = other.Owner.transform.TransformPoint(other._boxParam.Center);
        Vector3 pos = Owner.transform.TransformPoint(_center);
        return Vector3.Distance(otherPos, pos) <= (other._boxParam.Ray + _largestDist);
    }

    private bool HitBoundBox(CompCollision other)
    {
        // transform box param from other to our local space
        Vector3 otherCenter = other._boxParam.Center;
        otherCenter = other.Owner.transform.TransformPoint(otherCenter);

        // our local space
        otherCenter = Owner.transform.InverseTransformPoint(otherCenter);

        BoxParam tmp = new BoxParam(otherCenter);
        tmp.x.Clamp = new Vector2(otherCenter.x - other._boxParam.x.Half, otherCenter.x + other._boxParam.x.Half);
        tmp.y.Clamp = new Vector2(otherCenter.y - other._boxParam.y.Half, otherCenter.y + other._boxParam.y.Half);
        tmp.z.Clamp = new Vector2(otherCenter.z - other._boxParam.z.Half, otherCenter.z + other._boxParam.z.Half);

        return _boxParam.HasContact(tmp);
    }
    
    private void DestroyCollider(CompCollision other)
    {
    	EntBullet ent = other.Owner.GetComponent<EntBullet>();
    	if(ent)
    	{
    		int dmg = ent.ParamAttribut.Damage;
    		EntShield our = Owner.GetComponent<EntShield>();
    		if(our != null)
    		{
    			
    		}
    
    		// to do dmg shield
    		Builder.Instance.DestroyGameObject(ent.gameObject);
    	}
    }
}
    
