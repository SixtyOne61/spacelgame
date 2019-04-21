using UnityEngine;

namespace Tool
{
    [CreateAssetMenu(fileName = "Scripable Player Controller", menuName = "Scriptable/Scripable Player Controller")]
    public class SCRController : ScriptableObject
    {
        public AnimationCurve AccCurve;
        public float Acc;
        public AnimationCurve SpeedCurve;
        public float Speed;
        public float AngularSpeed;
    }
}
