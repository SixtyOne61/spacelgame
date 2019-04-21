using UnityEngine;

namespace Tool
{
    [CreateAssetMenu(fileName = "Scriptable UI Button", menuName = "Scriptable/UI/Button")]
    public class SCRUIBaseButton : ScriptableObject
    {
        [Tooltip("position of button")]
        public Vector2 Position;
    }
}