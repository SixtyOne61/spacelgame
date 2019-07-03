﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Engine
{
    public class VolumeEntity : VisuelEntity
    {
        // Component
        public CompMeshGenerator CompMeshGenerator = new CompMeshGenerator();
        public CompMaterial CompMaterial = new CompMaterial();

        [Tooltip("param, each Volume entity can make damage")]
        public Tool.SCRCubeAttribut ParamAttribut;

        [HideInInspector]
        public List<LinkPos> LinkPosList = new List<LinkPos>();

        // set true for refresh on next frame
        protected bool _flagRefresh = false;

        // debug
        protected List<LinkPos> _debugRemove = new List<LinkPos>();

        public override void Start()
        {
            CompMeshGenerator.LinkPosList = LinkPosList;

            AddComponent(CompMeshGenerator);
            AddComponent(CompMaterial);
            base.Start();

            GetComponent<MeshFilter>().mesh = CompMeshGenerator.CustomMesh;
        }

        public virtual void Refresh()
        {
            CompMeshGenerator.GenerateMesh();
        }

        public override void Update()
        {
            base.Update();

            if (_flagRefresh)
            {
                Refresh();
                _flagRefresh = false;
            }
        }

        public virtual void RemoveAt(int index, int dmg)
        {
            LinkPos remove = LinkPosList.ElementAt(index);

            remove.Life -= dmg;

            // we don't remove this component, we have life
            if (remove.Life > 0)
            {
                return;
            }

            foreach (KeyValuePair<LinkPos.Neighbor, UnitPos> neigbor in remove.Neighbors)
            {
                int invert = (int)neigbor.Key * -1;
                SearchNeigbhor(neigbor.Value, invert, index, -1);
                SearchNeigbhor(neigbor.Value, invert, index, 1);
            }

            LinkPosList.RemoveAt(index);
            _flagRefresh = true;
        }

        public virtual void SearchNeigbhor(UnitPos lfv, int remove, int idx, int delta)
        {
            if (idx < 0 || idx >= LinkPosList.Count)
            {
                return;
            }

            LinkPos link = LinkPosList.ElementAt(idx);
            if (link.Center == lfv)
            {
                link.Remove((LinkPos.Neighbor)remove);
            }
            else
            {
                SearchNeigbhor(lfv, remove, idx + delta, delta);
            }
        }
    }
}