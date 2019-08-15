using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Engine
{
    [RequireComponent(typeof(MeshCollider))]
    public class CollideEntity : VolumeEntity
    {
        // need it for whale, export fail without array
        public LinkPos[] LinkPosArray;
        public LinkPos[] OuterPosArray;
        // mesh collider on this entity
        public MeshCollider MyMeshCollider;

        public override void Start()
        {
            if (LinkPosArray.Length != 0)
            {
                LinkPosList = LinkPosArray.ToList();
            }

            if(OuterPosArray.Length != 0)
            {
                _outerPos = OuterPosArray.ToList();
            }

            base.Start();

            // init mesh collider
            MyMeshCollider = GetComponent<MeshCollider>();
            if (MyMeshCollider)
            {
                MyMeshCollider.convex = false;
                MyMeshCollider.isTrigger = false;
                RefreshColliderMesh();
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            // check if object was destroy
            Alive();
            if(MyMeshCollider)
            {
                RefreshColliderMesh();
            }
        }
        
        private void RefreshColliderMesh()
        {
            MyMeshCollider.cookingOptions = MeshColliderCookingOptions.None;
            MyMeshCollider.sharedMesh = CompMeshGenerator.CustomMesh;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public void OnCollisionEnter(Collision other)
        {
            if (gameObject.tag.GetHashCode() == other.gameObject.tag.GetHashCode())
            {
                return;
            }

            ContactPoint[] points = new ContactPoint[other.contactCount];
            other.GetContacts(points);

            int dmg = other.gameObject.GetComponent<CollideEntity>().ParamAttribut.Damage;

            Hit(points, dmg);
        }
        
        public virtual void Hit(ContactPoint[] _points, int _dmg)
        {
        	int delta = Mathf.Max((int)Mathf.Ceil(-0.5f + Mathf.Sqrt(0.25f * LinkPosList.Count)), 1);
        	float cubeSize = CompMeshGenerator.ParamCubeSize.Value;
        	foreach(ContactPoint contactPoint in _points)
            {
            	Vector3 localPoint = transform.TransformPoint(contactPoint.point);
            	if(!RecursiveFind(0, delta, localPoint, cubeSize, _dmg, delta))
                {
                    Debug.Log("Hit without find");
                }
            }
        }
        
        public virtual bool RecursiveFind(int _start, int _end, Vector3 _pos, float _cubeSize, int _dmg, int _delta)
        {
        	if(_end >= LinkPosList.Count || LinkPosList[_end].Center.x + _cubeSize > _pos.x)
        	{
        		return Find(_start, _end, _pos, _cubeSize, _dmg);
        	}
        	else
        	{
        		_start += _delta;
        		_end += _delta;
        		return RecursiveFind(_start, _end, _pos, _cubeSize, _dmg, _delta);
        	}	
        }
        
        public virtual bool Find(int _start, int _end, Vector3 _pos, float _cubeSize, int _dmg)
        {
            bool ret = false;
            for(int i = _start; i < _end; ++i)
            {
                if (i < LinkPosList.Count)
                {
                    if (LinkPosList[i].HasContact(_pos, _cubeSize))
                    {
                        ret = true;
                        ApplyDmg(i, _dmg);
                    }
                }
                else
                {
                    return ret;
                }
            }
            return ret;
        }
    }
}
