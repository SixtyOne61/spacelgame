using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;

namespace Tool
{
    public class Builder : Singleton<Builder>
    {
        [Tooltip("Prefab for Ship")]
        public BuilderShip BuilderShip;

        [Tooltip("Prefab for UI")]
        public BuilderUI BuilderUI;

        [Tooltip("Prefab for Fx")]
        public BuilderFx BuilderFx;

        [Tooltip("Prefab for Gameplay")]
        public BuilderGameplay BuilderGameplay;

        [Tooltip("Prefab for World")]
        public BuilderWorld BuilderWorld;

        public enum FactoryType
        {
            UI,
            Fx,
            Ship,
            Gameplay,
            World,
        }

        public void Start()
        {
            BuilderShip.Init();
            BuilderUI.Init();
            BuilderFx.Init();
            BuilderGameplay.Init();
            BuilderWorld.Init();
        }

        public GameObject Build(FactoryType factoryType, int type, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject obj = null;

            switch (factoryType)
            {
                case FactoryType.UI:
                    obj = SpawnObj(BuilderUI.Build(type), position, rotation, parent);
                    break;

                case FactoryType.Fx:
                    obj = SpawnObj(BuilderFx.Build(type), position, rotation, parent);
                    break;

                case FactoryType.Ship:
                    obj = SpawnObj(BuilderShip.Build(type), position, rotation, parent);
                    break;

                case FactoryType.Gameplay:
                    obj = SpawnObj(BuilderGameplay.Build(type), position, rotation, parent);
                    break;

                case FactoryType.World:
                    obj = SpawnObj(BuilderWorld.Build(type), position, rotation, parent);
                    break;

                default:
                    Debug.LogError("Factory Type doesn't manage.");
                    break;
            }

            return obj;
        }

        static public GameObject SpawnEmpty(string name)
        {
            return new GameObject(name);
        }

        private GameObject SpawnObj(GameObject obj, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject spawned = (GameObject)Instantiate(obj, position, rotation);
            spawned.transform.SetParent(parent);

            return spawned;
        }

        public void DestroyGameObject(GameObject gameObject, bool isImmediate)
        {
            if (isImmediate)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}