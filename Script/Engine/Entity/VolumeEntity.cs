using System.Collections.Generic;
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
        [HideInInspector]
        public List<List<LinkPos>> SubPos = new List<List<LinkPos>>();
        // set true for refresh on next frame
        protected bool _flagRefresh = false;

        // debug
        protected List<LinkPos> _debugRemove = new List<LinkPos>();

        public override void Start()
        {
            // dispash linkpos list to sub list
            DispashLinkPos();
            CompMeshGenerator.LinkPosList = LinkPosList;
            CompMeshGenerator.SubPos = SubPos;

            AddComponent(CompMeshGenerator);
            AddComponent(CompMaterial);
            base.Start();

            GetComponent<MeshFilter>().mesh = CompMeshGenerator.CustomMesh;
        }

        private void DispashLinkPos()
        {
            int maxElem = Mathf.Max((int)Mathf.Ceil(-0.5f + Mathf.Sqrt(0.25f * LinkPosList.Count)), 1);
            int count = LinkPosList.Count;
            // sort linkPos
            LinkPosList.Sort((p1, p2) => p1.Center.x < p2.Center.y ? 1 : -1);

            for(int i = 0; i < count;)
            {
            	SubPos.Add(new List<LinkPos>());
            	int max = Mathf.Min(count - i, maxElem);
            	for(int j = 0; j < max; ++j)
            	{
                    if(i+j >= count)
                    {
                        break;
                    }
                    SubPos.Last().Add(LinkPosList[i+j]);
                }
                i += max;
            }
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

        // TO DO : facto with EntShipPart
        public virtual bool RemoveAt(int index, int dmg)
        {
            LinkPos remove = LinkPosList.ElementAt(index);

            remove.Life -= dmg;

            // we don't remove this component, we have life
            if (remove.Life > 0)
            {
                return false;
            }

            foreach (KeyValuePair<LinkPos.Neighbor, UnitPos> neigbor in remove.getNeighbor())
            {
                int invert = (int)neigbor.Key * -1;
                SearchNeigbhor(neigbor.Value, invert, index, -1);
                SearchNeigbhor(neigbor.Value, invert, index, 1);
            }

            LinkPosList.RemoveAt(index);
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
    }
}
    