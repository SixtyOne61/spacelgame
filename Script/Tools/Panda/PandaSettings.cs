using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public class PandaSettings : PandaHandleBase
    {
        // parent of camera
        public Transform Camera;
        // list of all weapon
        public List<Transform> ShootingsSpawn = new List<Transform>();
        // list of speed fx
        public List<Transform> SpeedFxSpawn = new List<Transform>();

        public override void SpawnIfNecessary()
        {
            if (Camera == null)
            {
                Camera = Builder.SpawnEmpty("Camera").transform;
            }
        }

        public override void DestroyIfNecessary()
        {
            if(Camera != null)
            {
                Builder.Instance.DestroyGameObject(Camera.gameObject, true);
            }
        }
    }
}