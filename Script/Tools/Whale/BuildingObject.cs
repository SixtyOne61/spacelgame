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
        // global box
        private Vector2 _bornx;
        private Vector2 _borny;
        private Vector2 _bornz;

        public BuildingObject(LinkPos pos, SCRNoise param)
        {
            Param = param;
            _desc.Add(pos);
            // init born
            _bornx = new Vector2(pos.Center.x, pos.Center.x);
            _borny = new Vector2(pos.Center.y, pos.Center.y);
            _bornz = new Vector2(pos.Center.z, pos.Center.z);
        }

        public bool Add(LinkPos pos)
        {
            if(IsInBorn(pos))
            {
                foreach (LinkPos validPos in _desc)
                {
                    if(validPos.Has(pos.Center))
                    {
                        _desc.Add(pos);
                        UpdateBorn(pos.Center.x, pos.Center.y, pos.Center.z);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsInBorn(LinkPos pos)
        {
            // cube size == 1, so + 1
            return (pos.Center.x >= _bornx.x - 1 && pos.Center.x <= _bornx.y + 1) ||
                (pos.Center.y >= _borny.x - 1 && pos.Center.y <= _borny.y + 1) ||
                (pos.Center.z >= _bornz.x - 1 && pos.Center.z <= _bornz.y + 1);
        }

        private void UpdateBorn(float x, float y, float z)
        {
            UpdateBorn(ref _bornx, x);
            UpdateBorn(ref _borny, y);
            UpdateBorn(ref _bornz, z);
        }

        private void UpdateBorn(ref Vector2 born, float val)
        {
            born.x = Mathf.Min(val, born.x);
            born.y = Mathf.Max(val, born.y);
        }

        public void ExportToPrefab()
        {
            GameObject obj = new GameObject();
            obj.AddComponent<VolumeEntity>();

            VolumeEntity volEnt = obj.GetComponent<VolumeEntity>();
            volEnt.LinkPosArray = _desc.ToArray();
            volEnt.ParamAttribut = Param.ParamAttribut;
            volEnt.CompMeshGenerator.ParamCubeSize = Param.ParamCubeSize;
            volEnt.CompMaterial.ParamMaterial = Param.ParamMaterial;

            string localPath = "Assets/Prefab/World/Generate/" + _desc[0].Center + ".prefab";
            PrefabUtility.SaveAsPrefabAssetAndConnect(obj, localPath, InteractionMode.AutomatedAction);
        }
    }
}