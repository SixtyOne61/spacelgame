using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;

public class WhaleBuild
{
    public SCRMap ParamMap;
    public SCRNoise ParamNoise;
    public SCROneValue ParamCubeWorldSize;
    public SCROneValue ParamNbChunck;

    private List<HelperGenerateWorld> m_rocks = new List<HelperGenerateWorld>();

    public void Generate()
    {
        m_rocks.Clear();
        // sub divise world
        int delta = (int)ParamNbChunck.Value;
        Vector3 chunckSize = new Vector3(ParamMap.Width / delta, ParamMap.Height / delta, ParamMap.Depth / delta);
        Vector3 origine = Vector3.zero;

        for(int i = 0; i < delta; ++i)
        {
            for (int j = 0; j < delta; ++j)
            {
                for (int z = 0; z < delta; ++z)
                {
                    GenerateSubPart(chunckSize, new Vector3(i * chunckSize.x, j * chunckSize.y, z * chunckSize.z));
                }
            }
        }
    }

    private void GenerateSubPart(Vector3 chunckSize, Vector3 origine)
    {
        HelperGenerateWorld Rock = new HelperGenerateWorld(ParamNoise.RockThreshold, ParamCubeWorldSize, origine);

        float sizeCubeWorld = ParamCubeWorldSize.Value;
        for (float x = 0; x < chunckSize.x; x += sizeCubeWorld)
        {
            for (float y = 0; y < chunckSize.y; y += sizeCubeWorld)
            {
                for (float z = 0; z < chunckSize.z; z += sizeCubeWorld)
                {
                    UnitPos pos = new UnitPos(x, y, z);
                    Rock.Add(pos);
                }
            }
        }

        // dispach all coord on unitary object (object who all coord can acces to an other)
        Rock.DispachPosInUnitaryObject();

        m_rocks.Add(Rock);
    }

    public void Export(string Name)
    {
		 XmlRW.Export(Name, m_rocks);
    }
    
    public void Load(string Name)
    {
    	XmlRW.Load(Name, ref m_rocks);
    }
}
