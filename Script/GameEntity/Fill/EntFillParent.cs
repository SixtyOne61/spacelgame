using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

public class EntFillParent : SpacelEntity
{
    public Transform PlayersParent;
    public Transform WorldParent;
    public Transform LootParent;

    public override void Start()
    {
        base.Start();
        GameManager.Instance.PlayersParent = PlayersParent;
        GameManager.Instance.WorldParent = WorldParent;
        GameManager.Instance.LootParent = LootParent;
        GameManager.Instance.Init();
    }
}
