using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    [SerializeField] private List<FinishLine> _finishLines;

    public float CurrentTime { get; private set; }

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
        CurrentTime = 0f;
    }

    private void StartTimer()
    {
        StartCoroutine(TimerRoutine());
    }

    private void StopTimer(float subframeRatio)
    {
        Debug.Log("STOP");
        StopAllCoroutines();

        // Compensate for the last iteration of TimerRoutine()
        CurrentTime -= Time.deltaTime;
        CurrentTime += subframeRatio * Time.deltaTime;

        LevelManager.Instance.SetState(LevelState.Finished);
    }

    public IEnumerator TimerRoutine()
    {
        while(true)
        {
            CurrentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
