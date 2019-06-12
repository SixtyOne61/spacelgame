using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public class WhaleBuild
    {
        [Tooltip("Param for build map")]
        public SCRWhale ParamWhale;
        [Tooltip("Noise rock param")]
        public SCRNoise ParamRock;

        // contains all rock helper
        private List<HelperBuildRock> _helperRocks = new List<HelperBuildRock>();

        public void Init()
        {
            _helpers.Clear();
            HelperBuildRock baseRock = new HelperBuildRock();
            baseRock.ParamWhale = ParamWhale;
            baseRock.ParamRock = ParamRock;
            for(int i = 0; i < ParamWhale.NbChunck * 3; ++i)
            {
            	_helperRocks.Add(new HelperBuildRock(baseRock)));
            }      
        }
        
        public void GenerateRock()
        {
        	Vector2 delta = new vector2(ParamWhale.SizeChunck, ParamWhale.SizeChunck);
        	int max = delta.x * ParamWhale.NbChunck;
        	Vector2 bornx = bornxDefault = new Vector2(0, ParamWhale.SizeChunck);
        	Vector2 borny = bornyDefault = new Vector2(0, ParamWhale.SizeChunck);
        	Vector2 bornz = bornzDefault = new Vector2(0, ParamWhale.SizeChunck);
        	foreach(HelperBuildRock helper in _helperRocks)
        	{
        		helper.bornx = bornx;
        		helper.borny = borny;
        		helper.bornz = bornz;
        		
        		helper.Generate();
        		helper.ExportToPrefab();
        		
        		// change born
        		bornx += delta;
        		if(bornx.y >= max)
        		{
        			bornx = bornxDefault;
        			borny += delta;
        			if(borny.y >= delta)
        			{
        				borny = bornyDefault;
        				bornz += delta;
        			}
        		}
        		
        	}
        }

        public void Generate()
        {
        	GenerateRock();
            // TO DO : change this, make escargot
            // generate all object for world, create prefab
            for (int x = ParamWhale.Width.x; x <= ParamWhale.Width.y; ++x)
            {
                for (int y = ParamWhale.Height.x; y <= ParamWhale.Height.y; ++y)
                {
                    for (int z = ParamWhale.Depth.x; z <= ParamWhale.Depth.y; ++z)
                    {
                        // ... call on all helper
                        foreach (AbsHelperBuild helper in _helpers)
                        {
                            helper.Build(x, y, z);
                        }
                    }
                }
            }

            // export each object to prefab
            foreach (AbsHelperBuild helper in _helpers)
            {
                helper.ExportToPrefab();
            }
        }

    }
}
