using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Engine;

namespace Tool
{
    public class BuildingObject
    {
        [Tooltip("Noise param")]
        public SCRNoise Param;

        // list of pos
        private List<LinkPos> _desc = new List<LinkPos>();

        public BuildingObject(SCRNoise param)
        {
            Param = param;
        }

        public bool Add(LinkPos pos)
        {
            _desc.Add(pos);
            return true;
        }

        public void ExportToPrefab()
        {
            GameObject obj = new GameObject();
            obj.AddComponent<CollideEntity>();

            CollideEntity collideEntity = obj.GetComponent<CollideEntity>();
            collideEntity.LinkPosArray = _desc.ToArray();
            collideEntity.ParamAttribut = Param.ParamAttribut;
            collideEntity.CompMeshGenerator.ParamCubeSize = Param.ParamCubeSize;
            collideEntity.CompMaterial.ParamMaterial = Param.ParamMaterial;

            string localPath = "Assets/Resources/Generate/" + _desc[0].Center + ".prefab";
            PrefabUtility.SaveAsPrefabAssetAndConnect(obj, localPath, InteractionMode.AutomatedAction);
        }
    }
}