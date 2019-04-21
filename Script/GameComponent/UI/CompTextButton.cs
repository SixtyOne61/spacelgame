using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [System.Serializable]
    public class CompTextButton : CompBaseButton
    {
        [Tooltip("param for button")]
        public Tool.SCRUITextButton ParamButtom;

        public override void Start()
        {
            _param = ParamButtom;
            base.Start();
            // override text
            Button.GetComponentInChildren<Text>().text = ParamButtom.Text;
        }
    }
}