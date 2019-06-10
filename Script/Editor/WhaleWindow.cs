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
        }

        private void DisplaySettings()
        {
            _whaleBuild.ParamWhale = EditorGUILayout.ObjectField("Param Whale", _whaleBuild.ParamWhale, typeof(SCRWhale), false) as SCRWhale;
            _whaleBuild.ParamRock = EditorGUILayout.ObjectField("Param Rock", _whaleBuild.ParamRock, typeof(SCRNoise), false) as SCRNoise;
            
        }

        private void DisplayActions()
        {
            Label("Actions");

            if (AddButton("Generate"))
            {
                _whaleBuild.Init(); // TO DO : change this
                _whaleBuild.Generate();
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
