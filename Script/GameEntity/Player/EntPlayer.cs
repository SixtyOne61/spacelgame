using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using Tool;

public class EntPlayer : SpacelEntity
{
    public CompController ComponentController;
    public CompShooter ComponentShooter;

    // camera's player
    [HideInInspector]
    public GameObject Camera;

    // ressource for rebuild, this ressource was collect
    [HideInInspector]
    public float Ressource = 0.0f;

    // contains all part of a player
    private Dictionary<int, GameObject> _shipPartsObj = new Dictionary<int, GameObject>();

    public override void Start()
    {
        // add component
        AddComponent(ComponentController);
        AddComponent(ComponentShooter);

        // spawn camera
        Camera = Builder.Instance.Build(Builder.FactoryType.Gameplay, (int)BuilderGameplay.Type.Camera, Vector3.zero, Quaternion.identity, transform);

        // list to spawn of shooting spawner
        List<Transform> shootingsSpawn = new List<Transform>();
        // list to spawn speed fx
        List<Transform> speedFx = new List<Transform>();

        // load xml
        Dictionary<int, Tool.ShipPart> shipParts = new Dictionary<int, Tool.ShipPart>();
        Tool.ShipXml.Load(name, ref shipParts, Camera.transform, transform, shootingsSpawn, speedFx);

        // set parent of shootings spawn
        SetParent(ref shootingsSpawn);
        SetParent(ref speedFx);

        SpawnShipPart(ref shipParts);
        base.Start();
    }

    private void SetParent(ref List<Transform> trs)
    {
        foreach (Transform tr in trs)
        {
            tr.parent = transform;
        }
    }

    private void SpawnShipPart(ref Dictionary<int, Tool.ShipPart> shipParts)
    {
        foreach(KeyValuePair<int, Tool.ShipPart> part in shipParts)
        {
            GameObject partObj = null;
            if (!_shipPartsObj.ContainsKey(part.Key))
            {
                //position of part
                Spawn(ref partObj, part.Value);
                // set name of part
                partObj.name = part.Value.Param.Type.ToString();

                // add to dictionnary
                _shipPartsObj.Add(part.Key, partObj);
            }
            else // for editor
            {
                partObj = _shipPartsObj[part.Key];
            }

            // init list of position in entity ship part
            EntShipPart entShipPart = partObj.GetComponent<EntShipPart>();
            entShipPart.Init(part.Value);
        }
    }

    private void Spawn(ref GameObject obj, Tool.ShipPart part)
    {
        switch(part.Param.Type)
        {
            case Tool.TypeCube.Body:
                obj = Builder.Instance.Build(Builder.FactoryType.Ship, (int)BuilderShip.Type.Body, Vector3.zero, Quaternion.identity, transform);
                break;

            case Tool.TypeCube.Cockpit:
                obj = Builder.Instance.Build(Builder.FactoryType.Ship, (int)BuilderShip.Type.Cockpit, Vector3.zero, Quaternion.identity, transform);
                break;

            case Tool.TypeCube.Gun:
                obj = Builder.Instance.Build(Builder.FactoryType.Ship, (int)BuilderShip.Type.Gun, Vector3.zero, Quaternion.identity, transform);
                break;

            case Tool.TypeCube.Power:
                obj = Builder.Instance.Build(Builder.FactoryType.Ship, (int)BuilderShip.Type.Power, Vector3.zero, Quaternion.identity, transform);
                break;

            default:
                Debug.LogError("Type of part ship doesn't support.");
                break;
        }
    }
}
