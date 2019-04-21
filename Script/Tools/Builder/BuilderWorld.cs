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
        [Tooltip("Chunck Prefab")]
        public GameObject ChunckPrefab;
        [Tooltip("Border Prefab")]
        public GameObject BorderPrefab;
        [Tooltip("World Prefab")]
        public GameObject WorldPrefab;

        public enum Type : int
        {
            Chunck,
            Rock,
            Border,
            World,
        }

        public void Init()
        {
            RockPrefab.tag = Tag;
            ChunckPrefab.tag = Tag;
            BorderPrefab.tag = Tag;
            WorldPrefab.tag = Tag;
        }

        public GameObject Build(int type)
        {
            GameObject obj = null;
            switch (type)
            {
                case (int)Type.Chunck:
                    obj = ChunckPrefab;
                    break;

                case (int)Type.Rock:
                    obj = RockPrefab;
                    break;

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
