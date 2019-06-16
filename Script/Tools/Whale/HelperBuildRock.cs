using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

namespace Tool
{
    [System.Serializable]
    public class HelperBuildRock : AbsHelperBuild
    {
        [Tooltip("Noise rock param")]
        public SCRNoise ParamRock;

        // valide position
        private List<BuildingObject> _objects = new List<BuildingObject>();
        private List<Vector3Int> _openList = new List<Vector3Int>();

        public HelperBuildRock(HelperBuildRock copy)
        {
            ParamRock = copy.ParamRock;
            _objects = copy._objects;
            _openList = copy._openList;

            ParamWhale = copy.ParamWhale;
            Bornx = copy.Bornx;
            Borny = copy.Borny;
            Bornz = copy.Bornz;
        }

        public HelperBuildRock(SCRNoise paramRock, SCRWhale paramWhale)
        {
            ParamWhale = paramWhale;
            ParamRock = paramRock;
        }

        public override void ExportToPrefab()
        {
            foreach(BuildingObject obj in _objects)
            {
                obj.ExportToPrefab();
            }
        }
        
        public override void Generate()
        {
            _openList = new List<Vector3Int>();

            for (int x = (int)Bornx.x; x <= (int)Bornx.y; ++x)
            {
                for (int y = (int)Borny.x; y <= (int)Borny.y; ++y)
                {
                    for (int z = (int)Bornz.x; z <= (int)Bornz.y; ++z)
                    {
                        if(IsValideNoise(x, y, z))
                        {
                            _openList.Add(new Vector3Int(x, y, z));
                        }
                    }
                }
            }

            while(_openList.Count != 0)
            {
                int x = _openList[0].x;
                int y = _openList[0].y;
                int z = _openList[0].z;
                LinkPos newpos = new LinkPos(new UnitPos(x, y, z));
                _openList.RemoveAt(0);

                // create new object
                _objects.Add(new BuildingObject(ParamRock));
                // all neighboor
                AddNeighboor(x, y + 1, z, LinkPos.Neighbor.Top, ref newpos);
                AddNeighboor(x, y - 1, z, LinkPos.Neighbor.Bottom, ref newpos);
                AddNeighboor(x + 1, y, z, LinkPos.Neighbor.Right, ref newpos);
                AddNeighboor(x - 1, y, z, LinkPos.Neighbor.Left, ref newpos);
                AddNeighboor(x, y, z + 1, LinkPos.Neighbor.Back, ref newpos);
                AddNeighboor(x, y, z - 1, LinkPos.Neighbor.Front, ref newpos);

                _objects[_objects.Count - 1].Add(newpos);
            }
        }

        private void Add(int x, int y, int z)
        {
            LinkPos newpos = new LinkPos(new UnitPos(x, y, z));
            // all neighboor
            AddNeighboor(x, y + 1, z, LinkPos.Neighbor.Top, ref newpos);
            AddNeighboor(x, y - 1, z, LinkPos.Neighbor.Bottom, ref newpos);
            AddNeighboor(x + 1, y, z, LinkPos.Neighbor.Right, ref newpos);
            AddNeighboor(x - 1, y, z, LinkPos.Neighbor.Left, ref newpos);
            AddNeighboor(x, y, z + 1, LinkPos.Neighbor.Back, ref newpos);
            AddNeighboor(x, y, z - 1, LinkPos.Neighbor.Front, ref newpos);

            _objects[_objects.Count - 1].Add(newpos);
        }

        private bool IsValideNoise(int x, int y, int z)
        {
            float noise = Math.PerlinNoise3D(x, y, z, 0.020f, 4);
            return noise >= ParamRock.Threshold.x && noise <= ParamRock.Threshold.y;
        }

        private void AddNeighboor(int x, int y, int z, LinkPos.Neighbor where, ref LinkPos pos)
        {
            if(IsValidCoord(x, y, z) && IsValideNoise(x, y, z))
            {
                pos.Add(where, new UnitPos(x, y, z));
                // remove from open list, and add
                if(_openList.Remove(new Vector3Int(x, y, z)))
                {
                    // add in current object
                    Add(x, y, z);
                }
            }
        }
    }
}
