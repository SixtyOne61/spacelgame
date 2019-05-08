using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tool
{
    public class WhaleWindow : EditorWindow
    {
        // name of export file
        public string NameExport = "Name Export";

        // manage workd
        private WhaleBuild _whaleBuild = new WhaleBuild();

        [MenuItem("SpaceL/Whale Window")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(WhaleWindow));
        }

        public void OnGUI()
        {
            DisplaySettings();
            DisplayActions();
            DisplayExport();
        }

        private void DisplaySettings()
        {
            _whaleBuild.ParamMap = EditorGUILayout.ObjectField("Param Map", _whaleBuild.ParamMap, typeof(SCRMap), false) as SCRMap;
            _whaleBuild.ParamNoise = EditorGUILayout.ObjectField("Param Noise", _whaleBuild.ParamNoise, typeof(SCRNoise), false) as SCRNoise;
            _whaleBuild.ParamCubeWorldSize = EditorGUILayout.ObjectField("Param Cube World Size", _whaleBuild.ParamCubeWorldSize, typeof(SCROneValue), false) as SCROneValue;
            _whaleBuild.ParamNbChunck = EditorGUILayout.ObjectField("Param Nb Chunck", _whaleBuild.ParamNbChunck, typeof(SCROneValue), false) as SCROneValue;
        }

        private void DisplayActions()
        {
            Label("Actions");

            if (AddButton("Generate"))
            {
                _whaleBuild.Generate();
            }
        }

        private void DisplayExport()
        {
            Label("Export");

            NameExport = TextField(NameExport);

            if(AddButton("Export"))
            {
                _whaleBuild.Export(NameExport);
            }

            if(AddButton("Load"))
            {
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

        private bool AddButton(string name)
        {
            return GUILayout.Button(name);
        }
    }
}
