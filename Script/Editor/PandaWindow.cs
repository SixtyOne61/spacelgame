using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tool
{
    public class PandaWindow : EditorWindow
    {
        public static int NameHash = ("PandaCursor").GetHashCode();

        // true if we want auto snap, in fact, all time use it
        public bool AutoSnap = true;
        // true if we want populate mesh easier
        public bool EditOnContinue = false;
        // name of export file
        public string NameExport = "Name Export";
        // nb shooting point
        public int NbShootingPoint = 0;
        // nb fx speed
        public int NbFxSpeed = 0;

        // previous position of cursor
        private Vector3 _prevPosition = Vector3.zero;

        // manage ship
        private PandaBuild _pandaBuild = new PandaBuild();
        // settings for ship
        private PandaSettings _pandaSettings = new PandaSettings();

        [MenuItem("SpaceL/Panda Window")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(PandaWindow));
        }

        public void OnGUI()
        {
            DisplaySettings();
            DisplayActions();
            DisplayExport();
        }

        public void Update()
        {
            if (AutoSnap
                && !EditorApplication.isPlaying
                && Selection.transforms.Length > 0
                && _pandaBuild.ParamCube != null)
            {
                Snap();
            }
        }

        private void Snap()
        {
            foreach(Transform transform in Selection.transforms)
            {
                // snap only for our cursor
                if(transform.name.GetHashCode() == NameHash
                    && _prevPosition != transform.position)
                {
                    Vector3 pos = transform.position;
                    pos.x = Round(pos.x);
                    pos.y = Round(pos.y);
                    pos.z = Round(pos.z);

                    // force update position
                    transform.position = pos;
                    _prevPosition = transform.position;

                    if(EditOnContinue)
                    {
                        _pandaBuild.AddCube(pos);
                    }
                }
            }
        }

        private float Round(float val)
        {
            return _pandaBuild.ParamCube.SquareSize * Mathf.Round(val / _pandaBuild.ParamCube.SquareSize);
        }

        private void DisplaySettings()
        {
            Label("Settings");

            AutoSnap = Toggle("Auto Snap", AutoSnap);
            EditOnContinue = Toggle("Edit On Continue", EditOnContinue);

            // field for parent cube
            _pandaBuild.ShipParent = ObjectTransform("Ship Parent", _pandaBuild.ShipParent, true);

            // camera
            _pandaSettings.Camera = ObjectTransform("Camera", _pandaSettings.Camera, true);

            // nb shooting point  
            IntField("Nb Shooting point", ref NbShootingPoint);

            // resize list if necessary
            if(NbShootingPoint < _pandaSettings.ShootingsSpawn.Count)
            {
                _pandaSettings.ShootingsSpawn.RemoveRange(NbShootingPoint, _pandaSettings.ShootingsSpawn.Count - NbShootingPoint);
            }
            else if(NbShootingPoint > _pandaSettings.ShootingsSpawn.Count)
            {
                int nbAdd = NbShootingPoint - _pandaSettings.ShootingsSpawn.Count;
                for(int i = 0; i < nbAdd; ++i)
                {
                    _pandaSettings.ShootingsSpawn.Add(null);
                }
            }

            for (int i = 0; i < NbShootingPoint; ++i)
            {
                _pandaSettings.ShootingsSpawn[i] = ObjectTransform("Shooting spawn " + i, _pandaSettings.ShootingsSpawn[i], true);
            }

            // nb fx speed
            IntField("Nb Fx speed", ref NbFxSpeed);

            // resize list if necessary
            if (NbFxSpeed < _pandaSettings.SpeedFxSpawn.Count)
            {
                _pandaSettings.SpeedFxSpawn.RemoveRange(NbFxSpeed, _pandaSettings.SpeedFxSpawn.Count - NbFxSpeed);
            }
            else if (NbFxSpeed > _pandaSettings.SpeedFxSpawn.Count)
            {
                int nbAdd = NbFxSpeed - _pandaSettings.SpeedFxSpawn.Count;
                for (int i = 0; i < nbAdd; ++i)
                {
                    _pandaSettings.SpeedFxSpawn.Add(null);
                }
            }

            for (int i = 0; i < NbFxSpeed; ++i)
            {
                _pandaSettings.SpeedFxSpawn[i] = ObjectTransform("Fx speed spawn " + i, _pandaSettings.SpeedFxSpawn[i], true);
            }

            EditorGUI.BeginChangeCheck();
            // field for param cube
            _pandaBuild.ParamCube = EditorGUILayout.ObjectField("Param Cube", _pandaBuild.ParamCube, typeof(ScriptableCube), false) as ScriptableCube;

            // if param cube was change, we reset editor
            if (EditorGUI.EndChangeCheck())
            {
                Reset();
            }
        }

        private void DisplayActions()
        {
            Label("Actions");

            if (AddButton("Add"))
            {
                _pandaBuild.AddCube(_prevPosition);
            }

            if (AddButton("Remove"))
            {
                _pandaBuild.RemoveCube(_prevPosition);
            }

            if(AddButton("Clean"))
            {
                _pandaBuild.Clean();
            }
        }

        private void DisplayExport()
        {
            Label("Export");

            NameExport = TextField(NameExport);

            if(AddButton("Export"))
            {
                // export all ship part in xml
                // key was invalid
                _pandaBuild.SortLocation();
                XmlRW.Export(_pandaBuild.ShipPartEntities, NameExport, _pandaSettings.Camera, _pandaBuild.ShipParent, _pandaSettings.ShootingsSpawn, _pandaSettings.SpeedFxSpawn);
            }

            if(AddButton("Load"))
            {
                LoadFile();
            }
        }

        private Transform ObjectTransform(string name, Transform obj, bool allowSceneObject)
        {
            return EditorGUILayout.ObjectField(name, obj, typeof(Transform), allowSceneObject) as Transform;
        }

        private void Label(string label)
        {
            GUILayout.Label(label, EditorStyles.boldLabel);
        }

        private bool Toggle(string name, bool val)
        {
            return EditorGUILayout.Toggle(name, val);
        }

        private string TextField(string name)
        {
            return GUILayout.TextField(name);
        }

        private void IntField(string name, ref int val)
        {
            val = EditorGUILayout.IntField(name, val);
        }

        private void Reset()
        {
            // reset selection
            foreach (Transform transform in Selection.transforms)
            {
                // reset only cursor
                if (transform.name.GetHashCode() == NameHash)
                {
                    // force update position
                    transform.position = Vector3.zero;
                    _prevPosition = Vector3.zero;
                }
            }

            // reset edit on continue
            EditOnContinue = false;
        }

        private bool AddButton(string name)
        {
            return GUILayout.Button(name);
        }

        private void LoadFile()
        {
            _pandaBuild.Clean();

            // load part from file
            Dictionary<int, Tool.ShipPart> shipParts = new Dictionary<int, Tool.ShipPart>();

            // check if we must spawn camera, ship parent
            _pandaSettings.SpawnIfNecessary();
            _pandaBuild.SpawnIfNecessary();

            XmlRW.Load(NameExport, ref shipParts, _pandaSettings.Camera, _pandaBuild.ShipParent, _pandaSettings.ShootingsSpawn, _pandaSettings.SpeedFxSpawn);

            // refresh nb shooting spawn
            NbShootingPoint = _pandaSettings.ShootingsSpawn.Count;
            // refresh nb speed fx
            NbFxSpeed = _pandaSettings.SpeedFxSpawn.Count;

            _pandaBuild.CreateParts(ref shipParts);
        }
    }
}
