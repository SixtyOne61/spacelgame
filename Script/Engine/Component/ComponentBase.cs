using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    [System.Serializable]
    public class ComponentBase
    {
        [HideInInspector]
        public GameObject Owner;

        [HideInInspector]
        public bool Active = true;

        // Use this for initialization
        virtual public void Start()
        {

        }

        // Update is called once per frame
        virtual public void Update()
        {

        }

        virtual public void FixedUpdate()
        {

        }

#if (UNITY_EDITOR)
        virtual public void OnDrawGizmos()
        {

        }
#endif

        public ComponentBase SetOwner(GameObject aOwner)
        {
            Owner = aOwner;
            return this;
        }

        virtual public void OnDestroy()
        {

        }
    }
}
