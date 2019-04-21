using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Engine;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public abstract class EntBaseMenu : SpacelEntity
    {
        [Tooltip("Component Button")]
        public CompTextButton ComponentButton;

        public override void Start()
        {
            AddComponent(ComponentButton);
            base.Start();

            //add listener of button
            ComponentButton.Button.GetComponent<Button>().onClick.AddListener(OnValidate);
        }

        protected abstract void OnValidate();
    }
}
