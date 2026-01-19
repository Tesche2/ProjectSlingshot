using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
public enum LevelState { Overview, ZoomingIn, Countdown, Gameplay, LevelMenu, Finished }

public class LevelManager : MonoBehaviour
{
    private readonly static WaitForSeconds _waitForSeconds1 = new(1);
    public static LevelManager Instance;

    public LevelState currentState { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerEffects playerEffects;
    [SerializeField] private FinishLine finishLine;

    private Vector3 _spawnPosition;
    private float _ZoomInTime;
    private float _lineRatio;

    public System.Action OnOverviewStart;
    public System.Action OnZoomInStart;
    public System.Action OnCountdownStart;
    public System.Action OnGameplayStart;
    public System.Action OnMenuStart;
    public System.Action<string> OnCountdownMessage;

    public float timeValue {  get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Camera cam = Camera.main;
        _ZoomInTime = cam.GetComponent<CinemachineBrain>().DefaultBlend.Time;
    }

    private void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("Player NULL");
        }
        _spawnPosition = player.transform.position;
        EnterState(LevelState.Overview);

        timeValue = 0f;
    }

    private void OnEnable()
    {
        GameInput.LevelOverviewActions overviewActions = GlobalInputManager.Instance.InputActions.LevelOverview;
        overviewActions.StartGame.performed += _ => EnterState(LevelState.ZoomingIn);

        finishLine.OnFinishLineCrossed += EndGame;
    }

    public void EnterState(LevelState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case LevelState.Overview:
                GlobalInputManager.Instance.SetInputState_Overview();
                OnOverviewStart?.Invoke();
                break;

            case LevelState.ZoomingIn:
                GlobalInputManager.Instance.SetInputState_Blocked();
                OnZoomInStart?.Invoke();
                StartCoroutine(ZoomInRoutine());
                break;

            case LevelState.Countdown:
                GlobalInputManager.Instance.SetInputState_Blocked();
                OnCountdownStart?.Invoke();
                StartCoroutine(CountdownRoutine());
                break;

            case LevelState.Gameplay:
                GlobalInputManager.Instance.SetInputState_Gameplay();
                OnGameplayStart?.Invoke();
                StartCoroutine(TimerRoutine());
                break;

            case LevelState.LevelMenu:
                GlobalInputManager.Instance.SetInputState_Menu();
                OnMenuStart?.Invoke();
                break;
        }
    }

    public void InstantRestart()
    {
        // Stops countdown coroutine if it's already running
        StopAllCoroutines();
        ResetPlayer();
        timeValue = 0f;
        EnterState(LevelState.Countdown);
    }

    private void ResetPlayer()
    {
        player.transform.position = _spawnPosition;
        player.transform.rotation = Quaternion.identity;
        player.ResetPhysics();
        playerEffects.StopThrusterEffects();
    }

    private void EndGame(float lineRatio)
    {
        _lineRatio = lineRatio;
        EnterState(LevelState.Finished);
    }

    private IEnumerator ZoomInRoutine()
    {
        WaitForSeconds wait = new(_ZoomInTime);
        yield return wait;

        EnterState(LevelState.Countdown);
    }

    private IEnumerator CountdownRoutine()
    {
        OnCountdownMessage?.Invoke("3");
        yield return _waitForSeconds1;
        OnCountdownMessage?.Invoke("2");
        yield return _waitForSeconds1;
        OnCountdownMessage?.Invoke("1");
        yield return _waitForSeconds1;

        EnterState(LevelState.Gameplay);
    }

    public IEnumerator TimerRoutine()
    {
        while(currentState != LevelState.Finished)
        {
            timeValue += Time.deltaTime;

            yield return timeValue;
        }

        timeValue += Time.deltaTime * _lineRatio;

        Debug.Log(timeValue);
    }
}
