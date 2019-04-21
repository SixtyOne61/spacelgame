using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [System.Serializable]
    public class CompPlayerButton : CompBaseButton
    {
        [Tooltip("param for button")]
        public Tool.SCRUIBaseButton ParamButtom;

        public override void Start()
        {
            _param = ParamButtom;
            _type = Tool.BuilderUI.Type.BtnPlayer;
            base.Start();
        }
    }
}