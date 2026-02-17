using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
// Renders a dashed outline around the gravity influence region, ensuring dash length remains constant regardless of camera ortographic size.
public class OutlineRenderer : MonoBehaviour
{
    [SerializeField] private GravityObjectConfig config;

    private LineRenderer _lineRenderer;
    private Camera _cam;
    private float _baseCircumference;
    private GameObject _player;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.loop = true;

        _cam = Camera.main;

        _player = FindFirstObjectByType<PlayerController>().gameObject;

        // Get the circumference of the region based on its scale
        _baseCircumference = Mathf.PI * transform.localScale.x * transform.parent.localScale.x;

        DrawCircle();
    }

    private void Update()
    {
        // Ensure line width remains constant regardless of camera distance
        _lineRenderer.startWidth = config.baseOutlineWidth * _cam.orthographicSize / config.baseCameraOrthographicSize;

        // Rotate outline towards the player, this helps hide the fact that dashes spawn and disappear all in the same spot
        Vector3 directionToPlayer = _player.transform.position - transform.position;
        transform.right = directionToPlayer;

        // Find the size of the circumference on the screen
        float _visualCircumference = _baseCircumference * config.baseCameraOrthographicSize / _cam.orthographicSize;

        // Calculate the number of dashes that should be displayed and scale texture accordingly
        float _numberOfDashes = Mathf.Max(config.minOutlineSegments, (int) (_visualCircumference / config.baseOutlineDashLength));
        _lineRenderer.textureScale = new Vector2(_numberOfDashes / _baseCircumference, 0);
    }

    private void DrawCircle()
    {
        _lineRenderer.positionCount = config.outlineSegments;
        float anglePerStep = 360f / config.outlineSegments;

        for (int i = 0; i < config.outlineSegments; i++)
        {
            float angle = i * anglePerStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * 0.5f;
            float y = Mathf.Sin(angle) * 0.5f;

            _lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
