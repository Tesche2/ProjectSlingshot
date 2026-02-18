using UnityEngine;

public class LevelInputManager : MonoBehaviour
{
    private void OnEnable()
    {
        LevelManager.Instance.OnOverview += EnableOverviewControls;
        LevelManager.Instance.OnZoomIn += DisableControls;
        LevelManager.Instance.OnCountdownStart += DisableControls;
        LevelManager.Instance.OnGameplay += EnableGameplayControls;
        LevelManager.Instance.OnPlayerDead += EnableOverviewControls;

        LevelUIManager.Instance.OnMenuOpen += EnableMenuControls;

        GameInput.LevelOverviewActions overviewActions = GlobalInputManager.Instance.InputActions.LevelOverview;
        overviewActions.StartGame.performed += _ => LevelManager.Instance.EndOverview();

        GameInput.GameplayActions gameplayActions = GlobalInputManager.Instance.InputActions.Gameplay;
        gameplayActions.RestartLevel.performed += _ => LevelManager.Instance.InstantRestart();
        gameplayActions.OpenMenu.performed += _ => LevelManager.Instance.ToggleLevelMenu(true);

        GameInput.MenuNavigationActions menuActions = GlobalInputManager.Instance.InputActions.MenuNavigation;
        menuActions.Return.performed += _ => HandleMenuClosurePostFinish();
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnOverview -= EnableOverviewControls;
        LevelManager.Instance.OnZoomIn -= DisableControls;
        LevelManager.Instance.OnCountdownStart -= DisableControls;
        LevelManager.Instance.OnGameplay -= EnableGameplayControls;

        LevelUIManager.Instance.OnMenuOpen -= EnableMenuControls;
    }

    private void EnableOverviewControls()
    {
        GlobalInputManager.Instance.SetInputState_Overview();
    }

    private void EnableMenuControls()
    {
        GlobalInputManager.Instance.SetInputState_Menu();
    }

    private void EnableGameplayControls()
    {
        GlobalInputManager.Instance.SetInputState_Gameplay();
    }

    private void DisableControls()
    {
        GlobalInputManager.Instance.SetInputState_Blocked();
    }

    private void HandleMenuClosurePostFinish()
    {
        if (LevelManager.Instance.CurrentState == LevelState.LevelMenu)
        {
            LevelManager.Instance.ToggleLevelMenu(false);
        }
    }
}
