using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

namespace UI
{
    public class EntFillCanvas : SpacelEntity
    {
        [Tooltip("List of available menu")]
        public List<UIManager.View> AvailableView = new List<UIManager.View>();

        [Tooltip("First view displayed")]
        public UIManager.View First = UIManager.View.None;

        public override void Start()
        {
            base.Start();
            UIManager.Instance.Canvas = GetComponent<Canvas>();
            UIManager.Instance.AvailableView = AvailableView;
            UIManager.Instance.First = First;
            UIManager.Instance.Init();
        }
    }
}
