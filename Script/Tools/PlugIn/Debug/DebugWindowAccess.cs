using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if (UNITY_EDITOR) 
namespace Tool
{
    public class DebugWindowAccess : Singleton<DebugWindowAccess>
    {
        private DebugWindowSerialize _serialize = new DebugWindowSerialize(); // object serialized, contain info
        public DebugWindowSerialize Serialize
        {
            get { return _serialize; }
            set { _serialize = value; }
        }

        void Start()
        {
            Serialize = Serializer.Load<DebugWindowSerialize>("Assets/Serialize/DebugWindow.sa");
        }
    }
}
#endif
