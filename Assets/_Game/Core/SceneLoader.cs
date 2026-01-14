using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private string _activeScene;

    public System.Action OnMenuLoad;
    public System.Action OnLevelLoad;

    private void Awake()
    {
        if( Instance == null) Instance = this;
    }

    private void Start()
    {
        LoadScene(mainMenuSceneName);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(SceneChangeTask(sceneName));
    }

    public IEnumerator SceneChangeTask(string newScene)
    {
        if (!string.IsNullOrEmpty(_activeScene)) {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(_activeScene);

            while ( !unloadOp.isDone )
            {
                yield return null;
            }
        }

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

        while (!loadOp.isDone) {
            yield return null;
        }

        Scene scene = SceneManager.GetSceneByName(newScene);
        SceneManager.SetActiveScene(scene);

        if (newScene.StartsWith("Level_"))
        {
            OnLevelLoad?.Invoke();
        }
        else if (newScene == mainMenuSceneName)
        {
            OnMenuLoad?.Invoke();
        }

        _activeScene = newScene;
    }
}
