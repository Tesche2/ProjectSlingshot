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
        // Reset object
        float currentTime = 0;
        _splineAnimate.ElapsedTime = 0;
        float duration = _splineAnimate.Duration; // Ensure it respects the duration set in SplineAnimate

        while (currentTime < duration)
        {
            // Move object along the spline, reading the _positionCurve to make it easier to control the object's speed
            currentTime += Time.fixedDeltaTime;

            float normalizedTime = Mathf.Clamp01(currentTime / duration);
            float pos = _positionCurve.Evaluate(normalizedTime);

            _splineAnimate.ElapsedTime = pos * duration;
            
            yield return new WaitForFixedUpdate();
        }
        
        // Ensure it always reaches the end
        _splineAnimate.ElapsedTime = duration;
    }
}
