using UnityEngine;
using System.Linq;

namespace Engine
{
    [RequireComponent(typeof(MeshCollider))]
    public class CollideEntity : VolumeEntity
    {
        // TO DO : change this, use polymorphisme
        [Tooltip("True for static object")]
        public bool IsStaticObject = true;

        // need it for whale, export fail without array
        public LinkPos[] LinkPosArray;

        // component collision
        private ComponentCollision _componentCollision;

        public override void Start()
        {
            if (LinkPosArray.Length != 0)
            {
                LinkPosList = LinkPosArray.ToList();
            }

            _componentCollision = new ComponentCollision();
            AddComponent(_componentCollision);
            _componentCollision.LinkPosList = LinkPosList;
            _componentCollision.Init(CompMeshGenerator.ParamCubeSize.Value);

            // register
            if(IsStaticObject)
            {
                CollisionManager.Instance.RegisterStatic(_componentCollision);
            }
            else
            {
                CollisionManager.Instance.RegisterDynamic(_componentCollision);
            }

            GetComponent<MeshCollider>().sharedMesh = CompMeshGenerator.CustomMesh;
            base.Start();
        }

        public override void OnDestroy()
        {
            if(IsStaticObject)
            {
                CollisionManager.Instance.UnRegisterStatic(_componentCollision);
            }
            else
            {
                CollisionManager.Instance.UnRegisterDynamic(_componentCollision);
            }

            base.OnDestroy();
        }

        public override void Refresh()
        {
            base.Refresh();
            // TO DO
            //_componentCollision.Reset();

            // check if object was destroy
            Alive();
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

#if (UNITY_EDITOR)
        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (Tool.DebugWindowAccess.Instance.Serialize.EnableDrawRemovePos)
            {
                Gizmos.color = Color.magenta;
                Vector3 size = new Vector3(CompMeshGenerator.ParamCubeSize.Value, CompMeshGenerator.ParamCubeSize.Value, CompMeshGenerator.ParamCubeSize.Value);
                Vector3 delta = size / 2.0f;
                foreach (LinkPos pos in _debugRemove)
                {
                    Vector3 posVec = new Vector3(pos.Center.x + delta.x, pos.Center.y + delta.y, pos.Center.z + delta.z);
                    // add in world space
                    posVec += transform.position;
                    Gizmos.DrawWireCube(posVec, size);
                }
            }
        }
#endif
    }
}
