using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class SpacelEntity : MonoBehaviour
    {
        private List<ComponentBase> _components = new List<ComponentBase>();

        virtual public void Start()
        {
            foreach (ComponentBase comp in _components)
            {
                comp.Start();
            }
        }

        virtual public void Update()
        {
            foreach(ComponentBase comp in _components)
            {
                if(comp.Active)
                {
                    comp.Update();
                }
            }
        }

        virtual public void FixedUpdate()
        {
            foreach (ComponentBase comp in _components)
            {
                if(comp.Active)
                {
                    comp.FixedUpdate();
                }
            }
        }

#if (UNITY_EDITOR)
        virtual public void OnDrawGizmos()
        {
            foreach (ComponentBase comp in _components)
            {
                comp.OnDrawGizmos();
            }
        }
#endif

        public void AddComponent(ComponentBase aComponent)
        {
            _components.Add(aComponent.SetOwner(gameObject));
        }

        public void OnDestroy()
        {
            foreach (ComponentBase comp in _components)
            {
                comp.OnDestroy();
            }
        }
    }
}
