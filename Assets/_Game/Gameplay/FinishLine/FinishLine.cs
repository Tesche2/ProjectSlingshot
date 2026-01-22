using System;
using Unity.Cinemachine;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UIElements;

class VectorLine 
{
    public Vector2 Start { get; private set; }
    public Vector2 End { get; private set; }

    public VectorLine(Vector2 start, Vector2 end)
    {
        Start = start; End = end; 
    }

    public float Length()
    {
        return Vector2.Distance(Start, End);
    }

    public Vector2 FindIntersectionWith (VectorLine other)
    {
        return VectorUtils.IntersectLineSegments(Start, End, other.Start, other.End);
    }

    public void DrawLine(Color color)
    {
        Debug.DrawLine(Start, End, color, 100000000);
    }
}

public class FinishLine : MonoBehaviour
{
    private BoxCollider2D _finishLineCollider;
    private CircleCollider2D _playerCollider;

    private VectorLine[] _edges;

    public event Action<float> OnFinishLineCrossed;

    private void Awake()
    {
        _finishLineCollider = GetComponent<BoxCollider2D>();

        Vector2 localExtents = _finishLineCollider.bounds.size / 2f;
        Vector3 center = _finishLineCollider.bounds.center;
        Vector3 localBottomLeft = center + new Vector3(-localExtents.x, -localExtents.y, 0);
        Vector3 localBottomRight = center + new Vector3(localExtents.x, -localExtents.y, 0);
        Vector3 localTopLeft = center + new Vector3(-localExtents.x, localExtents.y, 0);
        Vector3 localTopRight = center + new Vector3(localExtents.x, localExtents.y, 0);

        VectorLine[] edgesArray = {
            new(localBottomLeft, localBottomRight),
            new(localBottomLeft, localTopLeft),
            new(localBottomRight, localTopRight),
            new(localTopLeft, localTopRight)
        };
        _edges = edgesArray;

        foreach (VectorLine edge in _edges) {
            edge.DrawLine(Color.blue);
        }

        LevelManager.Instance.OnGameplayStart += EnableCollider;
    }

    private void OnDestroy()
    {
        LevelManager.Instance.OnGameplayStart -= EnableCollider;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _playerCollider = other.GetComponent<CircleCollider2D>();
        HandleSubframeCalculation(other);
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
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller == null) return;

        // Find line for player trajectory
        VectorLine playerTrajectory = new(controller.PreviousFramePos, controller.transform.position);
        playerTrajectory.DrawLine(Color.red);

        Vector2 intersectionPoint = Vector2.zero;
        float shortestDistance = float.MaxValue;
        bool hasIntersection = false;

        foreach (VectorLine edge in _edges)
        {
            Vector2 crossPoint = playerTrajectory.FindIntersectionWith(edge);

            if (!float.IsInfinity(crossPoint.x))
            {
                float dist = Vector2.Distance(playerTrajectory.Start, crossPoint);
                if (dist < shortestDistance)
                {
                    shortestDistance = dist;
                    intersectionPoint = crossPoint;
                    hasIntersection = true;
                }
            }
        }

        if(hasIntersection)
        {
            _finishLineCollider.enabled = false;

            DrawX(intersectionPoint, Color.green);

            float totalLength = playerTrajectory.Length();
            float linesRatio = totalLength > Mathf.Epsilon ? shortestDistance / totalLength : 1f;

            OnFinishLineCrossed?.Invoke(linesRatio);
        }
    }

    private void DrawX(Vector3 position, Color color)
    {
        Debug.DrawLine(new Vector3(position.x - .1f, position.y - .1f), new Vector3(position.x + .1f, position.y + .1f), color, 100000000);
        Debug.DrawLine(new Vector3(position.x - .1f, position.y + .1f), new Vector3(position.x + .1f, position.y - .1f), color, 100000000);
    }
}
