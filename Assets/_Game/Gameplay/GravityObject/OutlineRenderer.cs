using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OutlineRenderer : MonoBehaviour
{
    [SerializeField] private GravityObjectConfig config;

    private LineRenderer _lineRenderer;
    private Camera _cam;
    private float _baseOrtographicSize;
    private int _numberOfDashes;
    private float _baseCircumference;
    private float _visualCircumference;
    private GameObject _player;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.loop = true;

        _cam = Camera.main;
        _baseOrtographicSize = 5;

        _player = FindFirstObjectByType<PlayerController>().gameObject;

        _baseCircumference = Mathf.PI * transform.localScale.x * transform.parent.localScale.x;

        DrawCircle();
    }

    private void Update()
    {
        _lineRenderer.startWidth = config.baseWidth * _cam.orthographicSize / _baseOrtographicSize;

        Vector3 directionToPlayer = _player.transform.position - transform.position;
        transform.right = directionToPlayer;

        _visualCircumference = _baseCircumference * _baseOrtographicSize / _cam.orthographicSize;
        _numberOfDashes = Mathf.Max(config.minSegments, (int) (_visualCircumference / config.baseDashLength));
        _lineRenderer.textureScale = new Vector2(_numberOfDashes / _baseCircumference, 0);
    }

    private void DrawCircle()
    {
        _lineRenderer.positionCount = config.segments;
        float anglePerStep = 360f / config.segments;

        for (int i = 0; i < config.segments; i++)
        {
            float angle = i * anglePerStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * 0.5f;
            float y = Mathf.Sin(angle) * 0.5f;

            _lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
