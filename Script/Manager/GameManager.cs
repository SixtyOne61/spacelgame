using System.Collections.Generic;
using UnityEngine;
using Tool;

public class GameManager : Singleton<GameManager>
{
    public Transform PlayersParent;
    public Transform WorldParent;
    public Transform LootParent;

    // list of all players
    [HideInInspector]
    public List<EntPlayer> Players = new List<EntPlayer>();

    public void Init()
    {
        // need to instantiate a player, init it and assign to ref transform on chunck manager
        SpawnWorld();
        SpawnPlayer();
        SpawnBorder();
    }

    private void SpawnPlayer()
    {
        GameObject localPlayer = Builder.Instance.Build(Builder.FactoryType.Gameplay, (int)Tool.BuilderGameplay.Type.Player, Vector3.zero, Quaternion.identity, PlayersParent);
        localPlayer.name = Utils.ShipUse.ShipName;
        // for switch chunck to the good one
        WorldParent.GetComponentInChildren<EntWorld>().RefTransform = localPlayer.transform;

        Players.Add(localPlayer.GetComponent<EntPlayer>());
    }

    private void SpawnBorder()
    {
        Builder.Instance.Build(Builder.FactoryType.World, (int)BuilderWorld.Type.Border, Vector3.zero, Quaternion.identity, WorldParent);
    }

    private void SpawnWorld()
    {
        Builder.Instance.Build(Builder.FactoryType.World, (int)BuilderWorld.Type.World, Vector3.zero, Quaternion.identity, WorldParent);
    }
}
