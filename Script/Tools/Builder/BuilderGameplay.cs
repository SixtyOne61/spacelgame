using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    [System.Serializable]
    public class BuilderGameplay
    {
        public static string Tag = "Player1";

        // prefab for gameplay
        [Tooltip("Player Prefab")]
        public GameObject PlayerPrefab;
        [Tooltip("Camera Prefab")]
        public GameObject CameraPrefab;
        [Tooltip("Bullet spawner Prefab")]
        public GameObject BulletSpawnerPrefab;
        [Tooltip("Bullet Prefab")]
        public GameObject BulletPrefab;
        [Tooltip("Shield Prefab")]
        public GameObject ShieldPrefab;

        public enum Type : int
        {
            Player,
            Camera,
            BulletSpawner,
            Bullet,
            Shield,
        }

        public void Init()
        {
            PlayerPrefab.tag = Tag;
            CameraPrefab.tag = Tag;
            BulletPrefab.tag = Tag;
            ShieldPrefab.tag = Tag;
        }

        public GameObject Build(int type)
        {
            GameObject obj = null;
            switch (type)
            {
                case (int)Type.Player:
                    obj = PlayerPrefab;
                    break;

                case (int)Type.Camera:
                    obj = CameraPrefab;
                    break;

                case (int)Type.BulletSpawner:
                    obj = BulletSpawnerPrefab;
                    break;

                case (int)Type.Bullet:
                    obj = BulletPrefab;
                    break;
                    
                case (int)Type.Bullet:
                	obj = ShieldPrefab;
                	break;

                default:
                    Debug.LogError("Type doesn't manage.");
                    break;
            }

            return obj;
        }
    }
}
