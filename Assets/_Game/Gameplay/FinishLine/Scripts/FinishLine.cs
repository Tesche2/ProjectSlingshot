using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class FinishLine : MonoBehaviour
{
    private BoxCollider2D _finishLineCollider;

    public event Action<float> OnFinishLineCrossed;

    private void Awake()
    { 
        _finishLineCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        LevelManager.Instance.OnGameplay += EnableCollider;
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnGameplay -= EnableCollider;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleSubframeCalculation(other);
        _finishLineCollider.enabled = false;
    }

    private void EnableCollider()
    {
        _finishLineCollider.enabled = true;
    }

    private void HandleSubframeCalculation(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerController>(out var controller)) return;

        LayerMask layerMask = LayerMask.GetMask("FinishLine");
        RaycastHit2D linecast = Physics2D.Linecast(controller.PreviousFramePos, other.transform.position, layerMask);

        float distanceTravelled = Vector2.Distance(controller.PreviousFramePos, other.transform.position);
        float distanceRatio = linecast.distance / distanceTravelled;

        OnFinishLineCrossed?.Invoke(distanceRatio);
    }
}
