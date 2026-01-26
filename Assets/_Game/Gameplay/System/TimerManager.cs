using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    [SerializeField] private List<FinishLine> _finishLines;

    private float _currentTime;
    public float DisplayTime { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnCountdownStart += ResetTimer;
            LevelManager.Instance.OnCountdownEnd += StartTimer;
        }

        foreach (var line in _finishLines)
        {
            line.OnFinishLineCrossed += StopTimer;
        }
    }

    private void OnDisable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnCountdownStart += ResetTimer;
            LevelManager.Instance.OnCountdownEnd += StartTimer;
        }

        foreach (var line in _finishLines)
        {
            line.OnFinishLineCrossed += StopTimer;
        }
    }

    private void ResetTimer()
    {
        StopAllCoroutines();
        _currentTime = 0f;
        DisplayTime = 0f;
    }

    private void StartTimer()
    {
        StartCoroutine(TimerRoutine());
        StartCoroutine(SmoothTimerRoutine());
    }

    private void StopTimer(float subframeRatio)
    {
        Debug.Log("STOP");
        StopAllCoroutines();

        // Compensate for the last iteration of TimerRoutine()
        _currentTime -= Time.deltaTime;
        _currentTime += subframeRatio * Time.deltaTime;
        DisplayTime = _currentTime;

        LevelManager.Instance.SetState(LevelState.Finished);
    }

    public IEnumerator TimerRoutine()
    {
        while(true)
        {
            _currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator SmoothTimerRoutine()
    {
        while(true)
        {
            float interpolation = Time.time - Time.fixedTime;
            DisplayTime = _currentTime + interpolation;
            yield return null;
        }
    }
}
