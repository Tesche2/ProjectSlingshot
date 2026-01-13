using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private readonly static WaitForSeconds _waitForSeconds1 = new(1);
    public static LevelManager Instance;

    public enum LevelState { Overview, Countdown, Gameplay, LevelMenu, Finished }
    public LevelState currentState {  get; private set; }

    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerEffects playerEffects;
    
    private Transform _spawnPoint;

    public System.Action OnOverviewStart;
    public System.Action OnCountdownStart;
    public System.Action OnGameplayStart;
    public System.Action OnMenuStart;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        _spawnPoint = player.transform;
        EnterState(LevelState.Overview);
    }

    public void EnterState(LevelState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case LevelState.Overview:
                player.SetInputActive(false);
                OnOverviewStart.Invoke();

                break;

            case LevelState.Countdown:
                ResetPlayer();
                player.SetInputActive(false);
                OnCountdownStart.Invoke();
                StartCoroutine(CountdownRoutine());

                break;

            case LevelState.Gameplay:
                player.SetInputActive(true);
                OnGameplayStart.Invoke();

                break;

            case LevelState.LevelMenu:
                player.SetInputActive(false);
                OnMenuStart.Invoke();

                break;
        }
    }

    public void InstantRestart()
    {
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
        Debug.Log("3...");
        yield return _waitForSeconds1;
        Debug.Log("2...");
        yield return _waitForSeconds1;
        Debug.Log("1...");
        yield return _waitForSeconds1;
        Debug.Log("GO!");

        EnterState(LevelState.Gameplay);
    }
}
