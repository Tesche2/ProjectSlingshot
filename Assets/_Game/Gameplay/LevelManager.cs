using System.Collections;
using UnityEngine;
public enum LevelState { Overview, Countdown, Gameplay, LevelMenu, Finished }

public class LevelManager : MonoBehaviour
{
    private readonly static WaitForSeconds _waitForSeconds1 = new(1);
    public static LevelManager Instance;

    public LevelState currentState {  get; private set; }

    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerEffects playerEffects;
    
    private Transform _spawnPoint;

    public System.Action OnOverviewStart;
    public System.Action OnCountdownStart;
    public System.Action OnGameplayStart;
    public System.Action OnMenuStart;
    public System.Action<string> OnCountdownMessage;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _spawnPoint = player.transform;

        if (GlobalInputManager.Instance != null)
        {
            GlobalInputManager.Instance.RegisterLevelEvents(this);
        }

        EnterState(LevelState.Overview);
    }

    private void OnDestroy()
    {
        if (GlobalInputManager.Instance != null)
        {
            GlobalInputManager.Instance.UnregisterLevelEvents(this);
        }
    }

    public void EnterState(LevelState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case LevelState.Overview:
                OnOverviewStart.Invoke();
                break;

            case LevelState.Countdown:
                ResetPlayer();
                OnCountdownStart.Invoke();
                StartCoroutine(CountdownRoutine());
                break;

            case LevelState.Gameplay:
                OnGameplayStart.Invoke();
                break;

            case LevelState.LevelMenu:
                OnMenuStart.Invoke();
                break;
        }
    }

    public void InstantRestart()
    {
        // Stops countdown coroutine if it's already running
        StopAllCoroutines();
        EnterState(LevelState.Countdown);
    }

    private void ResetPlayer()
    {
        player.transform.position = _spawnPoint.position;
        player.transform.rotation = _spawnPoint.rotation;
        player.ResetPhysics();
        playerEffects.StopThrusterEffects();
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
}
