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
        [Tooltip("Border Prefab")]
        public GameObject BorderPrefab;
        [Tooltip("World Prefab")]
        public GameObject WorldPrefab;

        public enum Type : int
        {
            Border,
            World,
        }

        public void Init()
        {
            BorderPrefab.tag = Tag;
            WorldPrefab.tag = Tag;
        }

        public GameObject Build(int type)
        {
            GameObject obj = null;
            switch (type)
            {
                case (int)Type.Border:
                    obj = BorderPrefab;
                    break;

                case (int)Type.World:
                    obj = WorldPrefab;
                    break;

                default:
                    Debug.LogError("Type doesn't manage.");
                    break;
            }

            return obj;
        }
    }
}
