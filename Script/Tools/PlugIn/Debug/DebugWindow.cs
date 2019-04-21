using System;
using UnityEngine;
using UnityEditor;

#if (UNITY_EDITOR) 
namespace Tool
{
    [Serializable]
    public class DebugWindowSerialize
    {
        public bool EnableDrawChunck = true;
        public bool EnableDrawBulletCollision = true;
        public bool EnableDrawRelativeBoxCollision = true;
        public bool EnableDrawRemovePos = true;
    }

    public class DebugWindow : EditorWindow
    {
        private DebugWindowSerialize _debugWindowSerialize = new DebugWindowSerialize(); // serialize class
        private GUIStyle _headerStyle; // header style's window
        private Vector2 _scrollPos; // scroll pos of the window


        //Add the tool to the Unity ToolBar 
        [MenuItem("Tools/Debug")]
        static void Init()
        {
            DebugWindow window = (DebugWindow)EditorWindow.GetWindow(typeof(DebugWindow));
            window.Show();
        }

        private void OnEnable()
        {
            //Styles 
            InitStyles();
            InitConfig();
        }

        private void OnDisable()
        {
            DebugWindowAccess.Instance.Serialize = _debugWindowSerialize;
            Serializer.Save<DebugWindowSerialize>("Assets/Serialize/DebugWindow.sa", _debugWindowSerialize);
        }

        private void OnGUI()
        {
            DrawMainCanvas();
        }

        private void InitStyles()
        {
            _headerStyle = new GUIStyle();
            _headerStyle.alignment = TextAnchor.MiddleCenter;
            _headerStyle.fontStyle = FontStyle.Bold;
        }

        private void InitConfig()
        {
            DebugWindowSerialize debugWindowSerialize = Serializer.Load<DebugWindowSerialize>("Assets/Serialize/DebugWindow.sa");

            if (debugWindowSerialize == null)
            {
                debugWindowSerialize = new DebugWindowSerialize();
            }

            _debugWindowSerialize = debugWindowSerialize;
        }

        private void DrawMainCanvas()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, false, false);
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical();

            GUILayout.Label("Enable debug", EditorStyles.boldLabel);

            _debugWindowSerialize.EnableDrawChunck = EditorGUILayout.Toggle("Draw Chunck", _debugWindowSerialize.EnableDrawChunck);
            _debugWindowSerialize.EnableDrawBulletCollision = EditorGUILayout.Toggle("Draw Bullet Collision", _debugWindowSerialize.EnableDrawBulletCollision);
            _debugWindowSerialize.EnableDrawRelativeBoxCollision = EditorGUILayout.Toggle("Draw Relative Box Collision", _debugWindowSerialize.EnableDrawRelativeBoxCollision);
            _debugWindowSerialize.EnableDrawRemovePos = EditorGUILayout.Toggle("Draw Remove Pos", _debugWindowSerialize.EnableDrawRemovePos);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
}
#endif