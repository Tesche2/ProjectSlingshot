using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Settings")]
    public string mainMenuSceneName = "MainMenu";
    public string coreUtilsScene = "Core_Utils";

    private string _activeScene;

    private void Awake()
    {
        if( Instance == null) Instance = this;
    }

    private void Start()
    {
        StartCoroutine(LoadMainMenu());
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log($"Changing to scene {sceneName}");
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
        _activeScene = newScene;
    }

    public IEnumerator LoadMainMenu()
    {
        yield return SceneManager.LoadSceneAsync(coreUtilsScene, LoadSceneMode.Additive);

        yield return SceneManager.LoadSceneAsync(mainMenuSceneName, LoadSceneMode.Additive);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainMenuSceneName));
        _activeScene = mainMenuSceneName;
    }
}
