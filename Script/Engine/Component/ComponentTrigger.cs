using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class ComponentTrigger : ComponentBase
    {
        private List<TriggerValue> _triggers = new List<TriggerValue>();

        public void AddTrigger(TriggerValue trigger)
        {

        }

        public override void Update()
        {
            base.Update();
            foreach(TriggerValue trigger in _triggers)
            {
                trigger.Update();
            }
        }
    }
}
