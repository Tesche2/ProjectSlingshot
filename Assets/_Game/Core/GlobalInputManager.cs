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

    public void SetInputState_Gameplay()
    {
        InputActions.Disable();
        InputActions.Gameplay.Enable();
        Debug.Log("GIM: Gameplay");
    }

    public void SetInputState_Menu()
    {
        InputActions.Disable();
        InputActions.MenuNavigation.Enable();
        Debug.Log("GIM: Menu");
    }

    public void SetInputState_Overview()
    {
        InputActions.Disable();
        InputActions.LevelOverview.Enable();
        Debug.Log("GIM: Overview");
    }

    public void SetInputState_Blocked()
    {
        InputActions.Disable();
        Debug.Log("GIM: Blocked");
    }
}
