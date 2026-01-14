using UnityEngine;

public class GlobalInputManager : MonoBehaviour
{
    public static GlobalInputManager Instance;

    public GameInput InputActions { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            InputActions = new GameInput();
            InputActions.Enable();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        if (SceneLoader.Instance == null) {
            Debug.LogWarning($"SCENE LOADER NULL");
        }

        SceneLoader.Instance.OnMenuLoad += EnableMenuControls;
        SceneLoader.Instance.OnLevelLoad += EnableOverviewControls;
    }

    private void OnDisable()
    {
        SceneLoader.Instance.OnMenuLoad -= EnableMenuControls;
        SceneLoader.Instance.OnLevelLoad -= EnableOverviewControls;
    }

    public void RegisterLevelEvents(LevelManager levelManager)
    {
        UnregisterLevelEvents(levelManager);

        levelManager.OnOverviewStart += EnableOverviewControls;
        levelManager.OnCountdownStart += DisableAllControls;
        levelManager.OnGameplayStart += EnableGameplayControls;
        levelManager.OnMenuStart += EnableMenuControls;
    }

    public void UnregisterLevelEvents(LevelManager levelManager)
    {
        levelManager.OnOverviewStart -= EnableOverviewControls;
        levelManager.OnCountdownStart -= DisableAllControls;
        levelManager.OnGameplayStart -= EnableGameplayControls;
        levelManager.OnMenuStart -= EnableMenuControls;
    }

    private void EnableGameplayControls()
    {
        InputActions.Disable();
        InputActions.Gameplay.Enable();
    }

    private void EnableMenuControls()
    {
        InputActions.Disable();
        InputActions.MenuNavigation.Enable();
    }

    private void EnableOverviewControls()
    {
        InputActions.Disable();
        InputActions.LevelOverview.Enable();
    }

    private void DisableAllControls()
    {
        InputActions.Disable();
    }
}
