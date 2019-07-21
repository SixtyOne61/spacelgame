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

        // component collision
        public ComponentCollision ComponentCollision;
        
        // mesh collider on this entity
        public MeshCollider MyMeshCollider;

        public override void Start()
        {
            if (LinkPosArray.Length != 0)
            {
                LinkPosList = LinkPosArray.ToList();
            }

            ComponentCollision = new ComponentCollision();
            AddComponent(ComponentCollision);
            ComponentCollision.LinkPosList = LinkPosList;
            ComponentCollision.Init(CompMeshGenerator.ParamCubeSize.Value);

            base.Start();

            // init mesh collider
            MyMeshCollider = GetComponent<MeshCollider>();
            if (MyMeshCollider)
            {
                MyMeshCollider.convex = true;
                MyMeshCollider.isTrigger = true;
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
        	MyMeshCollider.sharedMesh = CompMeshGenerator.CustomMesh;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public virtual void Alive()
        {
            // check if object need to be destroy
            if (LinkPosList.Count == 0)
            {
                Tool.Builder.Instance.DestroyGameObject(gameObject, false);
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if(gameObject.tag.GetHashCode() == other.gameObject.tag.GetHashCode())
            {
                return;
            }

            // for each link pos, try to remove
            for(int i = 0; i < LinkPosList.Count; )
            {
                Vector3 worldLocation = transform.TransformPoint(LinkPosList[i].Center.ToVec3());
                Vector3 closest = other.ClosestPoint(worldLocation);
                if(LinkPosList[i].HasContact(transform.InverseTransformPoint(closest), CompMeshGenerator.ParamCubeSize.Value))
                {
                    if(RemoveAt(i, ParamAttribut.Damage))
                    {
                        continue;
                    }
                }
                ++i;
            }
        }
    }
}
