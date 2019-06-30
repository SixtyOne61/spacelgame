using UnityEngine;
using System.Linq;

namespace Engine
{
    public class CollideEntity : VolumeEntity
    {
        [Tooltip("True for static object")]
        public bool IsStaticObject = true;

        // need it for whale, export fail without array
        public LinkPos[] LinkPosArray;

        // component collision
        private CompCollision _componentCollision;

        public override void Start()
        {
            if (LinkPosArray.Length != 0)
            {
                LinkPosList = LinkPosArray.ToList();
            }

            // init component collision
            if (IsStaticObject)
            {
                _componentCollision = new CompCollisionStatic();
            }
            else
            {
                _componentCollision = new CompCollisionDynamic();
            }

            _componentCollision.ParamCubeSize = CompMeshGenerator.ParamCubeSize;
            _componentCollision.LinkPosList = LinkPosList;
            AddComponent(_componentCollision);

            base.Start();
        }

        public override void Refresh()
        {
            base.Refresh();
            _componentCollision.Reset();

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
