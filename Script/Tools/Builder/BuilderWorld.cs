using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    [System.Serializable]
    public class BuilderWorld
    {
        public static string Tag = "World";

        // prefab for world
        [Tooltip("Rock Prefab")]
        public GameObject RockPrefab;
        [Tooltip("Border Prefab")]
        public GameObject BorderPrefab;
        [Tooltip("World Prefab")]
        public GameObject WorldPrefab;
        [Tooltip("Unit Collider Prefab")]
        public GameObject UnitColliderPrefab;

        public enum Type : int
        {
            Rock,
            Border,
            World,
            UnitCollider,
        }

        public void Init()
        {
            RockPrefab.tag = Tag;
            BorderPrefab.tag = Tag;
            WorldPrefab.tag = Tag;
            UnitColliderPrefab.tag = Tag;
        }

        public GameObject Build(int type)
        {
            GameObject obj = null;
            switch (type)
            {
                case (int)Type.Rock:
                    obj = RockPrefab;
                    break;

                case (int)Type.Border:
                    obj = BorderPrefab;
                    break;

                case (int)Type.World:
                    obj = WorldPrefab;
                    break;
                    
                case (int)Type.UnitCollider:
                	obj = UnitColliderPrefab;

                default:
                    Debug.LogError("Type doesn't manage.");
                    break;
            }

            return obj;
        }
    }
}
