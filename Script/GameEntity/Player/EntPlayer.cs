﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using Tool;

// TO DO : ent player must have a collider entity for all ship

public class EntPlayer : CollideEntity
{
    public CompController ComponentController;
    public CompShooter ComponentShooter;
    public CompSpecialist ComponentSpecialist;
    //public CompTakeOnMe ComponentTakeOnMe;

    // camera's player
    [HideInInspector]
    public GameObject Camera;

    // ressource for rebuild, this ressource was collect
    [HideInInspector]
    public float Ressource = 0.0f;
    
    // min max on each axis
    [HideInInspector]
    public Vector2 X;
    public Vector2 Y;
    public Vector2 Z;
    

    // contains all entity part of a player
    private Dictionary<int, EntShipPart> _shipPartsEntity = new Dictionary<int, EntShipPart>();

    public override void Start()
    {
        // add component
        AddComponent(ComponentController);
        AddComponent(ComponentShooter);
        AddComponent(ComponentSpecialist);
       // AddComponent(ComponentTakeOnMe);

        // spawn camera
        Camera = Builder.Instance.Build(Builder.FactoryType.Gameplay, (int)BuilderGameplay.Type.Camera, Vector3.zero, Quaternion.identity, transform);

        // list to spawn of shooting spawner
        List<Transform> shootingsSpawn = new List<Transform>();
        // list to spawn speed fx
        List<Transform> speedFx = new List<Transform>();

        // load xml
        Dictionary<int, Tool.ShipPart> shipParts = new Dictionary<int, Tool.ShipPart>();
        Tool.XmlRW.Load(name, ref shipParts, Camera.transform, transform, shootingsSpawn, speedFx);

        // set parent of shootings spawn
        SetParent(ref shootingsSpawn);
        // set parent of fx spawn
        SetParent(ref speedFx);

        SpawnShipPart(ref shipParts);
        base.Start();
        
        CombineMesh();
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
            EntShipPart partEntity = null;
            if (!_shipPartsEntity.ContainsKey(part.Key))
            {
                //position of part
                GameObject go = null;
                Spawn(ref go, part.Value);
                // set name of part
                go.name = part.Value.Param.Type.ToString();

                // add to dictionnary
                partEntity = go.GetComponent<EntShipPart>();
                // init list of position in entity ship part
                partEntity.Init(part.Value);

                // update min and max
                X.x = Mathf.Min(partEntity.X.x, X.x);
                X.y = Mathf.Max(partEntity.X.y, X.y);
                
                Y.x = Mathf.Min(partEntity.Y.x, Y.x);
                Y.y = Mathf.Max(partEntity.Y.y, Y.y);
                
                Z.x = Mathf.Min(partEntity.Z.x, Z.x);
                Z.y = Mathf.Max(partEntity.Z.y, Z.y);
                
                _shipPartsEntity.Add(part.Key, partEntity);
            }
            else // for editor
            {
                partEntity = _shipPartsEntity[part.Key];
                // init list of position in entity ship part
                partEntity.Init(part.Value);
            }
        }
        
        //CombineMesh();

        // ignore collision
        foreach(KeyValuePair<int, EntShipPart> shipPart in _shipPartsEntity)
        {
            if(shipPart.Value)
            {
                Collider colliderComp = shipPart.Value.GetComponent<Collider>();
                foreach (KeyValuePair<int, EntShipPart> shipPartOther in _shipPartsEntity)
                {
                    if(shipPartOther.Value == null || shipPart.Value.GetHashCode() == shipPartOther.Value.GetHashCode())
                    {
                        continue;
                    }
                    Physics.IgnoreCollision(colliderComp, shipPartOther.Value.GetComponent<Collider>());
                }
            }
        }
    }
    
    private void CombineMesh()
    {
    	MeshFilter[] meshs = new MeshFilter[_shipPartsEntity.Count];
    	foreach(KeyValuePair<int, EntShipPart> shipPart in _shipPartsEntity)
    	{
    		meshs[i].mesh = shipPart.Value.CompMeshGenerator.CustomMesh;
    		meshs[i].transform = shipPart.Value.transform.LocalToWorldMatrix;   		
    	}
    	MeshFilter meshFilter = GetComponent<MeshFilter>();
    	meshFilter.mesh = new Mesh();
    	meshFilter.mesh.CombineMeshes(meshs);
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
    
    public override void Update ()
    {
        base.Update();

        bool oneAlive = false;
        foreach (KeyValuePair<int, EntShipPart> part in _shipPartsEntity)
        {
            if(part.Value.PartIsAlive)
            {
                oneAlive = true;
            }
        }

        if(!oneAlive)
        {
            // TO DO :
            Debug.Log("Your ship is dead");
        }
    }
    
    public override bool RemoveAt(int index, int dmg)
    {
    }
}
    
    
