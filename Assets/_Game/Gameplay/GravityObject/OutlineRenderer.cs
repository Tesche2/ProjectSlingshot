using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(LineRenderer))]
public class OutlineRenderer : MonoBehaviour
{
    [SerializeField] private int segments = 20;
    [SerializeField] private float baseWidth = 0.1f;
    [SerializeField] private float baseRotationSpeed = 10f;

    private LineRenderer _lineRenderer;
    private Camera _cam;
    private float baseOrtographicSize;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.loop = true;

        _cam = Camera.main;
        baseOrtographicSize = _cam.orthographicSize;

        DrawCircle();
    }

    private void Update()
    {
        _lineRenderer.startWidth = baseWidth * _cam.orthographicSize / baseOrtographicSize;
        gameObject.transform.Rotate(new Vector3(0, 0, baseRotationSpeed * Time.deltaTime));
    }

    private void DrawCircle()
    {
        _lineRenderer.positionCount = segments;
        float anglePerStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * anglePerStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * 0.5f;
            float y = Mathf.Sin(angle) * 0.5f;

            _lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
