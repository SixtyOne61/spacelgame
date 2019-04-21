using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    [System.Serializable]
    public class BuilderUI
    {
        public static string Tag = "UI";

        // prefab for UI
        [Tooltip("Button template")]
        public GameObject BtnTemplatePrefab;
        [Tooltip("Button Player")]
        public GameObject BtnPlayerPrefab;
        [Tooltip("Lobby View")]
        public GameObject LobbyViewPrefab;
        [Tooltip("Ship Selection View")]
        public GameObject ShipSelectionViewPrefab;

        public enum Type : int
        {
            BtnTemplate,
            BtnPlayer,
            LobbyView,
            ShipSelectionView,
        }

        public void Init()
        {
            BtnTemplatePrefab.tag = Tag;
            BtnPlayerPrefab.tag = Tag;
            LobbyViewPrefab.tag = Tag;
            ShipSelectionViewPrefab.tag = Tag;
        }

        public GameObject Build(int type)
        {
            GameObject obj = null;
            switch (type)
            {
                case (int)Type.BtnTemplate:
                    obj = BtnTemplatePrefab;
                    break;

                case (int)Type.BtnPlayer:
                    obj = BtnPlayerPrefab;
                    break;

                case (int)Type.LobbyView:
                    obj = LobbyViewPrefab;
                    break;

                case (int)Type.ShipSelectionView:
                    obj = ShipSelectionViewPrefab;
                    break;

                default:
                    Debug.LogError("Type doesn't manage.");
                    break;
            }

            return obj;
        }
    }
}
