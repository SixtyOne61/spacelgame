using UnityEngine;

namespace Tool
{
    [CreateAssetMenu(fileName = "Scriptable Value Curve", menuName = "Scriptable/Scriptable Value Curve")]
    public class SCRValueCurve : ScriptableObject
    {
        public float Value;
        public AnimationCurve Curve;
    }
}