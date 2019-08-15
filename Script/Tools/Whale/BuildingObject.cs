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
        private List<LinkPos> _outer = new List<LinkPos>();

        public BuildingObject(SCRNoise param)
        {
            Param = param;
        }

        public bool Add(LinkPos pos)
        {
            _desc.Add(pos);
            if(!pos.IsSurrounded())
            {
                _outer.Add(pos);
            }
            return true;
        }

        private void Order()
        {
            // sort linkPos
            _desc.Sort((p1, p2) => p1.Center.x > p2.Center.x ? 1 : (p1.Center.x == p2.Center.x ? (p1.Center.y > p2.Center.y ? 1 : (p1.Center.y == p2.Center.y ? (p1.Center.z > p2.Center.z ? 1 : -1) : -1) ) : -1));
        }

        public void ExportToPrefab()
        {
            Order();

            GameObject obj = new GameObject();
            obj.AddComponent<CollideEntity>();

            CollideEntity collideEntity = obj.GetComponent<CollideEntity>();
            collideEntity.LinkPosArray = _desc.ToArray();
            collideEntity.OuterPosArray = _outer.ToArray();
            collideEntity.ParamAttribut = Param.ParamAttribut;
            collideEntity.CompMeshGenerator.ParamCubeSize = Param.ParamCubeSize;
            collideEntity.CompMaterial.ParamMaterial = Param.ParamMaterial;

            string localPath = "Assets/Resources/Generate/" + _desc[0].Center + ".prefab";
            PrefabUtility.SaveAsPrefabAssetAndConnect(obj, localPath, InteractionMode.AutomatedAction);
        }
    }
}