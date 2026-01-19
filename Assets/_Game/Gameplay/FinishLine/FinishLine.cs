using System;
using Unity.VectorGraphics;
using UnityEngine;

struct VectorLine 
{
    public Vector2 start;
    public Vector2 end;
}

public class FinishLine : MonoBehaviour
{
    private BoxCollider2D _finishLineCollider;
    private CircleCollider2D _playerCollider;

    private VectorLine _bottomEdge;

    public event Action<float> OnFinishLineCrossed;

    private void Awake()
    {
        _finishLineCollider = GetComponent<BoxCollider2D>();

        Vector2 localExtents = _finishLineCollider.bounds.size / 2f;
        Vector3 localBottomLeft = _finishLineCollider.bounds.center + new Vector3(-localExtents.x, -localExtents.y, 0);
        Vector3 localBottomRight = _finishLineCollider.bounds.center + new Vector3(localExtents.x, -localExtents.y, 0);

        Debug.Log($"Center: {_finishLineCollider.bounds.center}");
        Debug.Log($"left: {new Vector3(-localExtents.x, -localExtents.y, 0)}");
        Debug.Log($"Right: {new Vector3(localExtents.x, -localExtents.y, 0)}");

        _bottomEdge.start = localBottomLeft;
        _bottomEdge.end = localBottomRight;

        LevelManager.Instance.OnGameplayStart += EnableCollider;
    }

    private void OnDestroy()
    {
        LevelManager.Instance.OnGameplayStart -= EnableCollider;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _playerCollider = other.GetComponent<CircleCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        HandleSubframeCalculation(other);
    }

    private void EnableCollider()
    {
        _finishLineCollider.enabled = true;
    }

    private bool PlayerCrossedLine(Collider2D other)
    {
        Debug.Log($"Radius {_playerCollider.radius}");
        Debug.Log($"Distance: {other.Distance(_finishLineCollider).distance}");


        if (_playerCollider.radius + other.Distance(_finishLineCollider).distance < 0) return true;
        
        return false;
    }

    private void HandleSubframeCalculation(Collider2D other)
    {
        if(PlayerCrossedLine(other))
        {
            _finishLineCollider.enabled = false;

            PlayerController controller = other.GetComponent<PlayerController>();

            VectorLine playerTrajectory;
            playerTrajectory.start = controller.PreviousFramePos;
            playerTrajectory.end = controller.transform.position;

            Debug.DrawLine(_bottomEdge.start, _bottomEdge.end, Color.yellow);

            Vector2 crossingPoint = VectorUtils.IntersectLines(
                playerTrajectory.start,
                playerTrajectory.end,
                _bottomEdge.start,
                _bottomEdge.end
            );

            VectorLine trajectoryToLine;
            trajectoryToLine.start = playerTrajectory.start;
            trajectoryToLine.end = crossingPoint;

            float linesRatio = Vector2.Distance(trajectoryToLine.start, trajectoryToLine.end) / Vector2.Distance(playerTrajectory.start, playerTrajectory.end);
            OnFinishLineCrossed.Invoke(linesRatio);
        }
    }
}
