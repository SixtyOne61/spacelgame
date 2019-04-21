using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    [System.Serializable]
    public class BuilderShip
    {
        public static string Tag = "Player1";

        // prefab for ship
        [Tooltip("Body ship Prefab")]
        public GameObject BodyShipPrefab;
        [Tooltip("Gun ship Prefab")]
        public GameObject GunShipPrefab;
        [Tooltip("Cockpit ship Prefab")]
        public GameObject CockpitShipPrefab;
        [Tooltip("Power ship Prefab")]
        public GameObject PowerShipPrefab;
        [Tooltip("Removed ship Prefab")]
        public GameObject RemovedShipPrefab;

        public enum Type : int
        {
            Body,
            Gun,
            Cockpit,
            Power,
            RemovedShip,
        }

        public void Init()
        {
            BodyShipPrefab.tag = Tag;
            GunShipPrefab.tag = Tag;
            CockpitShipPrefab.tag = Tag;
            PowerShipPrefab.tag = Tag;
            RemovedShipPrefab.tag = Tag;
        }

        public GameObject Build(int type)
        {
            GameObject obj = null;
            switch (type)
            {
                case (int)Type.Body:
                    obj = BodyShipPrefab;
                    break;

                case (int)Type.Gun:
                    obj = GunShipPrefab;
                    break;

                case (int)Type.Cockpit:
                    obj = CockpitShipPrefab;
                    break;

                case (int)Type.Power:
                    obj = PowerShipPrefab;
                    break;

                case (int)Type.RemovedShip:
                    obj = RemovedShipPrefab;
                    break;

                default:
                    Debug.LogError("Type doesn't manage.");
                    break;
            }

            return obj;
        }
    }
}
