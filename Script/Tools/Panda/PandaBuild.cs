using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public class PandaBuild : PandaHandleBase
    {
        // Ship part will be spawn under this transform
        public Transform ShipParent;
        // Param cube of current cube
        public ScriptableCube ParamCube;

        // list of all part, Ship part contain information on each part
        public Dictionary<int, CollideEntity> ShipPartEntities = new Dictionary<int, CollideEntity>();

        public void AddCube(Vector3 position)
        {
            int hashId = GetHash();
            if (!ShipPartEntities.ContainsKey(hashId))
            {
                CreatePart(hashId);
            }

            Engine.LinkPos linkPos = new Engine.LinkPos(new UnitPos(position.x, position.y, position.z));

            if (!ShipPartEntities[hashId].LinkPosList.Contains(linkPos))
            {
                ShipPartEntities[hashId].LinkPosList.Add(linkPos);
                ShipPartEntities[hashId].Refresh();
            }
        }

        public void RemoveCube(Vector3 position)
        {
            int hashId = GetHash();
            // we know this ship part, we can remove
            if (ShipPartEntities.ContainsKey(hashId))
            {
                ShipPartEntities[hashId].LinkPosList.Remove(new Engine.LinkPos(new UnitPos(position.x, position.y, position.z)));
                ShipPartEntities[hashId].Refresh();
            }
        }

        public void Clean()
        {
            // clean all parts
            foreach (KeyValuePair<int, CollideEntity> part in ShipPartEntities)
            {
                Builder.Instance.DestroyGameObject(part.Value.gameObject, true);
            }
            ShipPartEntities.Clear();
        }

        private int GetHash() // to do deprecated ?
        {
            return ParamCube.GetUniqueId();
        }

        private void CreatePart(int hashId)
        {
            // spawn entity ship part
            GameObject obj = null;
            ShipPart part = new ShipPart(ParamCube);
            Spawn(ref obj, part);
            EntShipPart entity = obj.GetComponent<EntShipPart>();
            entity.Init(part);

            // force start on entity
            entity.Start();

            ShipPartEntities.Add(hashId, entity);
        }

        public void CreateParts(ref Dictionary<int, Tool.ShipPart> shipParts)
        {
            foreach (KeyValuePair<int, Tool.ShipPart> part in shipParts)
            {
                GameObject partObj = null;
                Spawn(ref partObj, part.Value);
                // set name of part
                partObj.name = part.Value.Param.Type.ToString();

                // init list of position in entity ship part
                EntShipPart entShipPart = partObj.GetComponent<EntShipPart>();
                entShipPart.Init(part.Value);

                // force start on entity
                entShipPart.Start();

                ShipPartEntities.Add(part.Key, entShipPart);
            }
        }

        public void Spawn(ref GameObject obj, ShipPart part)
        {
            switch (part.Param.Type)
            {
                case TypeCube.Body:
                    obj = Builder.Instance.Build(Builder.FactoryType.Ship, (int)BuilderShip.Type.Body, Vector3.zero, Quaternion.identity, ShipParent);
                    break;

                case TypeCube.Cockpit:
                    obj = Builder.Instance.Build(Builder.FactoryType.Ship, (int)BuilderShip.Type.Cockpit, Vector3.zero, Quaternion.identity, ShipParent);
                    break;

                case TypeCube.Gun:
                    obj = Builder.Instance.Build(Builder.FactoryType.Ship, (int)BuilderShip.Type.Cockpit, Vector3.zero, Quaternion.identity, ShipParent);
                    break;

                case TypeCube.Power:
                    obj = Builder.Instance.Build(Builder.FactoryType.Ship, (int)BuilderShip.Type.Power, Vector3.zero, Quaternion.identity, ShipParent);
                    break;

                default:
                    Debug.LogError("Type of part ship doesn't support.");
                    break;
            }
        }

        public override void SpawnIfNecessary()
        {
            if (ShipParent == null)
            {
                ShipParent = Builder.SpawnEmpty("ShipParent").transform;
            }
        }

        public override void DestroyIfNecessary()
        {
            if (ShipParent != null)
            {
                Builder.Instance.DestroyGameObject(ShipParent.gameObject, true);
            }
        }
    }
}
