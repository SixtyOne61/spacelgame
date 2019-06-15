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
            _helperRocks.Clear();
            HelperBuildRock baseRock = new HelperBuildRock();
            baseRock.ParamWhale = ParamWhale;
            baseRock.ParamRock = ParamRock;
                        
            for (int i = 0; i < ParamWhale.NbChunck; ++i)
            {
                for(int j = 0; j < ParamWhale.NbChunck; ++j)
                {
                    for(int k = 0; k < ParamWhale.NbChunck; ++k)
                    {
                        _helperRocks.Add(baseRock);
                    }
                }
            }   
        }
        
        public void GenerateRock()
        {
        	Vector2 delta = new Vector2(ParamWhale.SizeChunck, ParamWhale.SizeChunck);
        	int max = (int)(delta.x * ParamWhale.NbChunck);
        	Vector2 bornx = new Vector2(0, ParamWhale.SizeChunck);
            Vector2 bornxDefault = bornx;
            Vector2 borny = new Vector2(0, ParamWhale.SizeChunck);
            Vector2 bornyDefault = borny;
            Vector2 bornz = new Vector2(0, ParamWhale.SizeChunck);
            foreach (HelperBuildRock helper in _helperRocks)
        	{
        		helper.Bornx = bornx;
        		helper.Borny = borny;
        		helper.Bornz = bornz;
        		
        		helper.Generate();
        		helper.ExportToPrefab();
        		
        		// change born
        		bornx += delta;
        		if(bornx.y >= max)
        		{
        			bornx = bornxDefault;
        			borny += delta;
        			if(borny.y >= max)
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
        }
    }
}
