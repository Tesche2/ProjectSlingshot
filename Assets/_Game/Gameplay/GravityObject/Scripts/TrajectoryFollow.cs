using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineAnimate))]
public class TrajectoryFollow : MonoBehaviour
{
    [SerializeField] private AnimationCurve _positionCurve;
    private SplineAnimate _splineAnimate;

    private void Awake()
    {
        _splineAnimate = GetComponent<SplineAnimate>();
    }

    private void Start()
    {
        _splineAnimate.ElapsedTime = 0;
    }
    private void OnEnable()
    {
        if (LevelManager.Instance != null)
            LevelManager.Instance.OnCountdownStart += StartMovement;
    }

    private void OnDisable()
    {
        if (LevelManager.Instance != null)
            LevelManager.Instance.OnCountdownStart -= StartMovement;
    }

    private void StartMovement()
    {
        StopAllCoroutines();
        StartCoroutine(MoveObject());
    }

    private IEnumerator MoveObject()
    {
        // Ensure it respects the duration and loop mode set in SplineAnimate
        float duration = _splineAnimate.Duration;
        SplineAnimate.LoopMode loopMode = _splineAnimate.Loop;

        do
        {
            // Reset object
            float currentTime = 0;
            _splineAnimate.ElapsedTime = 0;

            while (currentTime < duration)
            {
                // Move object along the spline, reading the _positionCurve to make it easier to control the object's speed
                currentTime += Time.fixedDeltaTime;

                float normalizedTime = Mathf.Clamp01(currentTime / duration);
                float pos = _positionCurve.Evaluate(normalizedTime);

                _splineAnimate.ElapsedTime = pos * duration;
            
                yield return new WaitForFixedUpdate();
            }

        } while(loopMode == SplineAnimate.LoopMode.Loop);
        
        // Ensure it always reaches the end
        _splineAnimate.ElapsedTime = duration;
    }
}
