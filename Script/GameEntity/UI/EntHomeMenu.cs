using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace UI
{
    public class EntHomeMenu : EntBaseMenu
    {
        [Tooltip("Component Player Button")]
        public CompPlayerButton ComponentPlayerButton;

        public override void Start()
        {
            // override position
            RectTransform rect = GetComponent<RectTransform>();
            rect.anchoredPosition = Vector3.zero;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            // add component
            AddComponent(ComponentPlayerButton);
            base.Start();

            //add listener of button
            ComponentPlayerButton.Button.GetComponent<Button>().onClick.AddListener(OnClickPlayer);
        }

        private void OnClickPlayer()
        {
            UIManager.Instance.GoToView(UIManager.View.ShipSelection);
        }

        protected override void OnValidate()
        {
#if (UNITY_EDITOR)
            if (!EditorApplication.isPlaying)
            {
                return;
            }
#endif
            Utils.Loading.SceneName = "Game";
            SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Home");
        }
    }
}
