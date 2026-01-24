using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
public enum LevelState { Overview, ZoomingIn, Countdown, Gameplay, LevelMenu, Finished }

public class LevelManager : MonoBehaviour
{
    private readonly static WaitForSeconds _waitForSeconds3 = new(3);
    public static LevelManager Instance;

    public LevelState currentState { get; private set; }

    [Header("References")]
    [SerializeField] private FinishLine finishLine;

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
        currentState = newState;

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
        yield return _waitForSeconds3;
        OnCountdownEnd?.Invoke();
        SetState(LevelState.Gameplay);
    }

    public void ReturnToMainMenu()
    {
        SceneLoader.Instance.LoadScene(SceneLoader.Instance.mainMenuSceneName);
    }

    public void ToggleLevelMenu(bool active)
    {
        if (active) SetState(LevelState.LevelMenu);
        else SetState(LevelState.Gameplay);
    }

    public void EndOverview()
    {
        if(currentState == LevelState.Overview)
        {
            SetState(LevelState.ZoomingIn);
        }
    }
}
