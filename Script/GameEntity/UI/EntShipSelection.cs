using UnityEngine;
using UnityEditor;

namespace UI
{
    public class EntShipSelection : EntBaseMenu
    {
        public override void Start()
        {
            // override position
            RectTransform rect = GetComponent<RectTransform>();
            rect.anchoredPosition = Vector3.zero;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            base.Start();
        }

        protected override void OnValidate()
        {
#if (UNITY_EDITOR)
            if (!EditorApplication.isPlaying)
            {
                return;
            }
#endif
            UIManager.Instance.GoToView(UIManager.View.Lobby);
        }
    }
}
