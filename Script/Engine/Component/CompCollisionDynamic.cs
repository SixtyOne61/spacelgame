using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class CompCollisionDynamic : CompCollision
    {
        public override void Start()
        {
            base.Start();
            CollisionManager.Instance.Register(this);
        }
    }
}
