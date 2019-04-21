using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Engine;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class EntLoading : SpacelEntity
    {
        public override void Start()
        {
            // TO DO : add component widget loading
            base.Start();

            StartCoroutine(LoadAsyncScene());
        }

        public IEnumerator LoadAsyncScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(Utils.Loading.SceneName, LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            SceneManager.UnloadSceneAsync("Loading");
        }
    }
}
