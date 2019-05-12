using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using Tool;

public class EntChunck : SpacelEntity {

    public Tool.SCRMap ParamMap;
    public Tool.SCRNoise ParamNoise;
    public Tool.SCROneValue ParamCubeWorldSize;

    // list of all occuped position for each type
    public HelperGenerateWorld Rock;

    // Use this for initialization
    override public void Start()
    {
        StartCoroutine(GenerateHashWorldMap());
        base.Start();
    }

    private IEnumerator GenerateHashWorldMap()
    {
        Rock = new HelperGenerateWorld(ParamNoise.RockThreshold, ParamCubeWorldSize, transform.position);

        // for all chunck coord, check if we can add it in global groupe
        float sizeCubeWorld = ParamCubeWorldSize.Value;
        for (float x = 0; x < ParamMap.Width; x += sizeCubeWorld)
        {
            for (float y = 0; y < ParamMap.Height; y += sizeCubeWorld)
            {
                for (float z = 0; z < ParamMap.Depth; z += sizeCubeWorld)
                {
                    UnitPos pos = new UnitPos(x, y, z);
                    Rock.Add(pos);
                }
            }
        }

        // dispach all coord on unitary object (object who all coord can acces to an other)
        Rock.DispachPosInUnitaryObject();

        // spawn entity needed
        ReadForms();
        yield return null;
    }

    private void ReadForms()
    {
        foreach(List<LinkPos> linkPos in Rock.SubList)
        {
            // spawn mesh entity
            GameObject goMeshEntity = Builder.Instance.Build(Builder.FactoryType.World, (int)BuilderWorld.Type.Rock, transform.position, transform.rotation, transform);
            goMeshEntity.GetComponent<VolumeEntity>().LinkPosList = linkPos;
        }
    }
}
