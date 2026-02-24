using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
// Renders a dashed outline around the gravity influence region, ensuring dash length remains constant regardless of camera ortographic size.
public class OutlineRenderer : MonoBehaviour
{
    [SerializeField] private GravityObjectConfig _config;
    [SerializeField] private CameraConfig _camConfig;

    private LineRenderer _lineRenderer;
    private Camera _cam;
    private float _baseCircumference;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.loop = true;

        _cam = Camera.main;

        // Get the circumference of the region based on its scale
        _baseCircumference = Mathf.PI * transform.localScale.x * transform.parent.localScale.x;

        DrawCircle();
    }

    private void LateUpdate()
    {
        // Ensure line width remains constant regardless of camera distance
        _lineRenderer.startWidth = _config.baseOutlineWidth * _cam.orthographicSize;

        // Rotate outline towards the camera, this helps hide the fact that dashes spawn and disappear all in the same spot
        Vector2 directionToCamera = (Vector2) _cam.transform.position - (Vector2) transform.position;
        transform.right = directionToCamera;

        // Find the size of the circumference on the screen
        float visualCircumference = _baseCircumference / _cam.orthographicSize;

        // Calculate the number of dashes that should be displayed and scale texture accordingly
        float numberOfDashes = Mathf.Max(_config.minOutlineSegments, (int) (visualCircumference / _config.baseOutlineDashLength));
        _lineRenderer.textureScale = new Vector2(numberOfDashes / _baseCircumference, 1);
    }

    private void DrawCircle()
    {
        _lineRenderer.positionCount = _config.outlineSegments;
        float anglePerStep = 360f / _config.outlineSegments;

        for (int i = 0; i < _config.outlineSegments; i++)
        {
            float angle = i * anglePerStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * 0.5f;
            float y = Mathf.Sin(angle) * 0.5f;

            _lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
