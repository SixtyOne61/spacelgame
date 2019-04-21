using Engine;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EntAutoNextScene : SpacelEntity
{
    [Tooltip("Hide this canvas after load")]
    public GameObject HideAfterLoad;

    public override void Start()
    {
        base.Start();
        SceneManager.LoadScene("Home", LoadSceneMode.Additive);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.sceneCount > 1)
        {
            HideAfterLoad.SetActive(false);
        }
    }

    void OnSceneUnloaded(Scene current)
    {
        if (SceneManager.sceneCount == 1)
        {
            HideAfterLoad.SetActive(true);
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
