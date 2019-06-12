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

        public override void ExportToPrefab()
        {
            foreach(BuildingObject obj in _objects)
            {
                obj.ExportToPrefab();
            }
        }
        
        public override void Generate()
        {
        	int start = (bornx.y - bornx.x) >> 1;
        	int delta = 1;
        	while(delta != start)
        	{
        		for(int x = start - delta; x <= start + delta; x += delta)
        		++delta;
        	}
        	for(int x = bornx.x; x <= bornx.y; ++x)
        	{
        		for(int y = borny.x; y <= borny.y; ++y)
        		{
        			for(int z = bornz.x; z <= bornz.y; ++z)
        			{
        				Build(x,y,z);
        			}
        		}
        	}
        }

        public override void Build(int x, int y, int z)
        {
            if(IsValideNoise(x, y, z))
            {
                LinkPos newpos = new LinkPos(new UnitPos(x, y, z));
                // all neighboor
                AddNeighboor(x, y + 1, z, LinkPos.Neighbor.Top, ref newpos);
                AddNeighboor(x, y - 1, z, LinkPos.Neighbor.Bottom, ref newpos);
                AddNeighboor(x + 1, y, z, LinkPos.Neighbor.Right, ref newpos);
                AddNeighboor(x - 1, y, z, LinkPos.Neighbor.Left, ref newpos);
                AddNeighboor(x, y, z + 1, LinkPos.Neighbor.Back, ref newpos);
                AddNeighboor(x, y, z - 1, LinkPos.Neighbor.Front, ref newpos);

                // add in right object
                if(!AddInBuildingObject(ref newpos))
                {
                    // create new object
                    _objects.Add(new BuildingObject(newpos, ParamRock));
                }
            }
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
            }
        }

        private bool AddInBuildingObject(ref LinkPos pos)
        {
            foreach(BuildingObject obj in _objects)
            {
                if(obj.Add(pos))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
