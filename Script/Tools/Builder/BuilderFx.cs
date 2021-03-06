﻿using System.Collections;
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

        [Tooltip("Speed fx Prefab")]
        public GameObject SpeedFxPrefab;

        [Tooltip("Spark fx Prefab")]
        public GameObject SparkFxPrefab;

        public enum Type : int
        {
            Loot,
            Speed,
            Sparkle
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

                case (int)Type.Speed:
                    obj = SpeedFxPrefab;
                    break;

                case (int)Type.Sparkle:
                    obj = SparkFxPrefab;
                    break;

                default:
                    Debug.LogError("Type doesn't manage.");
                    break;
            }

            return obj;
        }
    }
}
