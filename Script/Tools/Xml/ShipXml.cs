using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Xml.Linq;
using System.Linq;

namespace Tool
{
    public class ShipXml
    {
        // from https://answers.unity.com/questions/1134997/string-to-vector3.html
        public static Vector3 StringToVector3(string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }

            // split the items
            string[] sArray = sVector.Split(',');

            // store as a Vector3
            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            return result;
        }

        public static void Export(Dictionary<int, CollideEntity> shipParts, string shipName, Transform cameraParent, Transform shipParent, List<Transform> shootingsSpawn, List<Transform> speedFxSpawn)
        {
            // add root in xml
            XDocument xmlDoc = new XDocument(new XElement("ShipDesc"));

            // for each ship part
            foreach (KeyValuePair<int, CollideEntity> part in shipParts)
            {
                // root of part
                XElement xmlPart = new XElement("Part");

                // Key of block
                xmlPart.Add(new XElement("Key", new XAttribute("Id", part.Key)));

                // all point
                xmlPart.Add(
                    new XElement("Map",
                        part.Value.LinkPosList.Select(x => new XElement("Data", new XAttribute("value", x.Center)))
                        )
                    );

                // add part in file
                xmlDoc.Root.Add(xmlPart);
            }

            // camera
            XElement xmlCamera = GetElemWithPositionAndForward("Camera", cameraParent.transform);
            // add camera in file
            xmlDoc.Root.Add(xmlCamera);

            // ship Parent
            XElement xmlShipParent = GetElemWithPosition("Pivot", shipParent.transform);
            // add pivot in file
            xmlDoc.Root.Add(xmlShipParent);

            // shooting spawn
            foreach(Transform shooting in shootingsSpawn)
            {
                XElement xmlShooting = GetElemWithPositionAndForward("Shooting", shooting);
                // add shooting in file
                xmlDoc.Root.Add(xmlShooting);
            }

            // speed fx
            foreach(Transform fx in speedFxSpawn)
            {
                XElement xmlfx = GetElemWithPositionAndForward("SpeedFx", fx);
                // add shooting in file
                xmlDoc.Root.Add(xmlfx);
            }

            xmlDoc.Save(Application.dataPath + "/Export/" + shipName + ".xml");
        }

        private static XElement GetElemWithPositionAndForward(string name, Transform tr)
        {
            XElement xmlElem = new XElement(name);
            xmlElem.Add(new XElement("Position", new XAttribute("x", tr.localPosition.x),
                                                        new XAttribute("y", tr.localPosition.y),
                                                        new XAttribute("z", tr.localPosition.z)));

            xmlElem.Add(new XElement("Forward", new XAttribute("x", tr.forward.x),
                                                        new XAttribute("y", tr.forward.y),
                                                        new XAttribute("z", tr.forward.z)));

            return xmlElem;
        }

        private static XElement GetElemWithPosition(string name, Transform tr)
        {
            XElement xmlElem = new XElement(name);
            xmlElem.Add(new XElement("Position", new XAttribute("x", tr.localPosition.x),
                                                        new XAttribute("y", tr.localPosition.y),
                                                        new XAttribute("z", tr.localPosition.z)));
            return xmlElem;
        }

        public static void Load(string shipName, ref Dictionary<int, ShipPart> shipParts, Transform cameraParent, Transform shipParent, List<Transform> shootingsSpawn, List<Transform> speedFxSpawn)
        {
            // list of scriptable
            List<ScriptableCube> scriptableCubeList = new List<ScriptableCube>();

            // find all scriptable cube
            string[] scriptables = AssetDatabase.FindAssets("Pa_");
            foreach(string scriptable in scriptables)
            {
                string path = AssetDatabase.GUIDToAssetPath(scriptable);
                scriptableCubeList.Add(AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableCube)) as ScriptableCube);
            }

            shipParts.Clear();
            XDocument xmlDoc = XDocument.Load(Application.dataPath + "/Export/" + shipName + ".xml");

            foreach (var xmlParts in xmlDoc.Root.Elements("Part"))
            {
                // key of this part
                int key = Int32.Parse(xmlParts.Element("Key").Attribute("Id").Value);

                // find the right scriptable cube
                foreach (ScriptableCube param in scriptableCubeList)
                {
                    if (param.GetUniqueId() == key)
                    {
                        shipParts.Add(key, new ShipPart(param));
                    }
                }

                // check ship param
                if (!shipParts.ContainsKey(key))
                {
                    Debug.LogError("Cube type : " + key + " was unknown.");
                    continue;
                }

                // add all point
                Vector3 pos = Vector3.zero;
                foreach (var elem in xmlParts.Element("Map").Elements("Data"))
                {
                    pos = ShipXml.StringToVector3(elem.Attribute("value").Value);
                    shipParts[key].Add(new UnitPos(pos.x, pos.y, pos.z));
                }
            }

            var xmlCamera = xmlDoc.Root.Element("Camera");
            cameraParent.localPosition = new Vector3(float.Parse(xmlCamera.Element("Position").Attribute("x").Value), float.Parse(xmlCamera.Element("Position").Attribute("y").Value), float.Parse(xmlCamera.Element("Position").Attribute("z").Value));
            cameraParent.forward = new Vector3(float.Parse(xmlCamera.Element("Forward").Attribute("x").Value), float.Parse(xmlCamera.Element("Forward").Attribute("y").Value), float.Parse(xmlCamera.Element("Forward").Attribute("z").Value));

            var xmlPivot = xmlDoc.Root.Element("Pivot");
            shipParent.position = new Vector3(float.Parse(xmlPivot.Element("Position").Attribute("x").Value), float.Parse(xmlPivot.Element("Position").Attribute("y").Value), float.Parse(xmlPivot.Element("Position").Attribute("z").Value));

            // load position of shootings spawn
            LoadList(ref shootingsSpawn, "Shooting", ref xmlDoc, Builder.FactoryType.Gameplay, (int)BuilderGameplay.Type.BulletSpawner);

            // TO DO : speed fx
            //LoadList(ref speedFxSpawn, "SpeedFx", ref xmlDoc, Builder.FactoryType.Fx, (int)BuilderFx.Type.Speed);
        }

        public static void ClearList(ref List<Transform> trs)
        {
            foreach (Transform tr in trs)
            {
                Builder.Instance.DestroyGameObject(tr.gameObject, true);
            }
            trs.Clear();
        }

        public static void LoadList(ref List<Transform> trs, string name, ref XDocument xmlDoc, Builder.FactoryType factype, int subtype)
        {
            ClearList(ref trs);
            int max = xmlDoc.Root.Elements(name).Count();
            for (int i = 0; i < max; ++i)
            {
                var xmlShoot = xmlDoc.Root.Elements(name).ElementAt(i);

                GameObject go = Builder.Instance.Build(factype, subtype, Vector3.zero, Quaternion.identity, null);

                go.transform.localPosition = new Vector3(float.Parse(xmlShoot.Element("Position").Attribute("x").Value), float.Parse(xmlShoot.Element("Position").Attribute("y").Value), float.Parse(xmlShoot.Element("Position").Attribute("z").Value));
                go.transform.forward = new Vector3(float.Parse(xmlShoot.Element("Forward").Attribute("x").Value), float.Parse(xmlShoot.Element("Forward").Attribute("y").Value), float.Parse(xmlShoot.Element("Forward").Attribute("z").Value));

                trs.Add(go.transform);
            }
        }
    }
}
