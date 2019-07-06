using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class CompCollisionStatic : CompCollision
    {
        public override void Start()
        {
            base.Start();
            CollisionManager.Instance.Register(this);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            CollisionManager.Instance.UnRegister(this);
        }
    }
}
