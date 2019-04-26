using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    [System.Serializable]
    public class BuilderFx
    {
        public static string Tag = "Fx";

        [Tooltip("Loot Prefab template")]
        public GameObject LootPrefab;

        [Tooltip("Trail bullet Prefab")]
        public GameObject TrailBulletPrefab;

        public enum Type : int
        {
            Loot,
            TrailBullet,
            Speed,
        }

        public void Init()
        {
            LootPrefab.tag = Tag;
        }

        public GameObject Build(int type)
        {
            GameObject obj = null;
            switch (type)
            {
                case (int)Type.Loot:
                    obj = LootPrefab;
                    break;

                case (int)Type.TrailBullet:
                    obj = TrailBulletPrefab;
                    break;

                default:
                    Debug.LogError("Type doesn't manage.");
                    break;
            }

            return obj;
        }
    }
}
