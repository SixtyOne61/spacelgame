using UnityEngine;
using Engine;
using Tool;

namespace UI
{
    [System.Serializable]
    public class CompBaseButton : ComponentBase
    {
        [HideInInspector]
        public GameObject Button;

        // type of this button, default was template button, can be override in child
        protected Tool.BuilderUI.Type _type = Tool.BuilderUI.Type.BtnTemplate;

        // base param button for position
        protected Tool.SCRUIBaseButton _param;

        public override void Start()
        {
            base.Start();
            Button = Builder.Instance.Build(Builder.FactoryType.UI, (int)_type, Vector3.zero, Quaternion.identity, Owner.transform);

            if(_param == null)
            {
                Debug.LogError("Param was not override in child");
                return;
            }

            // override position
            Button.GetComponent<RectTransform>().anchoredPosition = _param.Position;
        }
    }

}
