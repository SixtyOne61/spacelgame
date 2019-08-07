using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Engine
{
    // int maxElem = Mathf.Max((int)Mathf.Ceil(-0.5f + Mathf.Sqrt(0.25f * LinkPosList.Count)), 1);
    [RequireComponent(typeof(MeshFilter))]
    public class VolumeEntity : VisuelEntity
    {
        // Component
        public CompMeshGenerator CompMeshGenerator = new CompMeshGenerator();
        public CompMaterial CompMaterial = new CompMaterial();

        [Tooltip("param, each Volume entity can make damage")]
        public Tool.SCRCubeAttribut ParamAttribut;

        [HideInInspector]
        public List<LinkPos> LinkPosList = new List<LinkPos>();
        [HideInInspector]
        public List<List<LinkPos>> SubPos = new List<List<LinkPos>>();
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
        
        public virtual void ApplyDmg(int _index, int _dmg)
        {
        	LinkPos remove = LinkPosList.ElementAt(_index);

            remove.Life -= _dmg;

            // we don't remove this component, we have life
            if (remove.Life <= 0)
            {
                RemoveAt(_index, ref remove);
            }
        }

        public virtual bool RemoveAt(int _index, ref LinkPos _remove)
        {
            foreach (KeyValuePair<LinkPos.Neighbor, UnitPos> neigbor in _remove.getNeighbor())
            {
                int invert = (int)neigbor.Key * -1;
                SearchNeigbhor(neigbor.Value, invert, _index, -1);
                SearchNeigbhor(neigbor.Value, invert, _index, 1);
            }

            LinkPosList.RemoveAt(_index);
            _flagRefresh = true;
            return true;
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

        public virtual void Alive()
        {
            // check if object need to be destroy
            if (LinkPosList.Count == 0)
            {
                Tool.Builder.Instance.DestroyGameObject(gameObject, false);
            }
        }
    }
}
    
