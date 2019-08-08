﻿using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Engine
{
    [RequireComponent(typeof(MeshCollider))]
    public class CollideEntity : VolumeEntity
    {
        // need it for whale, export fail without array
        public LinkPos[] LinkPosArray;
                
        // mesh collider on this entity
        public MeshCollider MyMeshCollider;

        public override void Start()
        {
            if (LinkPosArray.Length != 0)
            {
                LinkPosList = LinkPosArray.ToList();
            }

            base.Start();

            // init mesh collider
            MyMeshCollider = GetComponent<MeshCollider>();
            if (MyMeshCollider)
            {
                MyMeshCollider.convex = true;
                MyMeshCollider.isTrigger = false;
                RefreshColliderMesh();
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            // check if object was destroy
            Alive();
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
            
            Hit(points);
        }
        
        public virtual void Hit(ContactPoint[] _points)
        {
        	foreach(ContactPoint contactPoint in _points)
            {
            	Vector3 localPoint = transform.TransformPoint(contactPoint.point);
            	int index = LinkPosList.Count / 2;
            	if(LinkPosList[index].Center.x + CubeSize < localPoint.x)
            	{
            		// recursive find index 
            	}
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if(gameObject.tag.GetHashCode() == other.gameObject.tag.GetHashCode())
            {
                return;
            }

            // for each link pos, try to remove
            for(int i = 0; i < LinkPosList.Count; ++i)
            {
                Vector3 worldLocation = transform.TransformPoint(LinkPosList[i].Center.ToVec3());
                Vector3 closest = other.ClosestPoint(worldLocation);
                if(LinkPosList[i].HasContact(transform.InverseTransformPoint(closest), CompMeshGenerator.ParamCubeSize.Value))
                {
                    ApplyDmg(i, ParamAttribut.Damage);
                }
            }
        }
    }
}
