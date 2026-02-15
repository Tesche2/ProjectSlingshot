using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
public enum LevelState { Overview, ZoomingIn, Countdown, Gameplay, LevelMenu, Finished }

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public LevelState CurrentState { get; private set; }

    private float _ZoomInTime;

    public System.Action OnOverview;
    public System.Action OnZoomIn;
    public System.Action OnCountdownStart;
    public System.Action OnCountdownEnd;
    public System.Action OnGameplay;
    public System.Action OnMenu;
    public System.Action OnFinished;
    public System.Action<string> OnCountdownMessage;

    public float TimeValue {  get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Camera cam = Camera.main;
        _ZoomInTime = cam.GetComponent<CinemachineBrain>().DefaultBlend.Time;
    }

    private void Start()
    {
        SetState(LevelState.Overview);
    }

    public void SetState(LevelState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case LevelState.Overview:
                OnOverview?.Invoke();
                break;

            case LevelState.ZoomingIn:
                OnZoomIn?.Invoke();
                StartCoroutine(ZoomInRoutine());
                break;

            case LevelState.Countdown:
                OnCountdownStart?.Invoke();
                StartCoroutine(CountdownRoutine());
                break;

            case LevelState.Gameplay:
                OnGameplay?.Invoke();
                break;

            case LevelState.LevelMenu:
                OnMenu?.Invoke();
                break;

            case LevelState.Finished:
                OnFinished?.Invoke();
                break;
        }
    }

    public void InstantRestart()
    {
        // Stops countdown coroutine if it's already running
        StopAllCoroutines();
        SetState(LevelState.Countdown);
    }

    private IEnumerator ZoomInRoutine()
    {
        WaitForSeconds wait = new(_ZoomInTime);
        yield return wait;

        SetState(LevelState.Countdown);
    }

    private IEnumerator CountdownRoutine()
    {
        for(int i = 3 * (int) (1.0f / Time.fixedDeltaTime); i > 0; i--)
        {
            yield return new WaitForFixedUpdate();
        }
        OnCountdownEnd?.Invoke();
        SetState(LevelState.Gameplay);
    }

    public void ReturnToMainMenu()
    {
        SceneLoader.Instance.LoadScene(SceneLoader.Instance.mainMenuSceneName);
    }

    public void ToggleLevelMenu(bool active)
    {
        if (CurrentState != LevelState.LevelMenu && CurrentState != LevelState.Gameplay) return;

        if (active) SetState(LevelState.LevelMenu);
        else SetState(LevelState.Gameplay);
    }

    public void EndOverview()
    {
        if(CurrentState == LevelState.Overview)
        {
            SetState(LevelState.ZoomingIn);
        }
    }
}
