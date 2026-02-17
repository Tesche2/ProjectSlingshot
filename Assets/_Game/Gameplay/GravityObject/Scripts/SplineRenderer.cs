using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(LineRenderer)), RequireComponent(typeof(SplineContainer))]
public class SplineRenderer : MonoBehaviour
{
    [SerializeField] private GravityObjectConfig config;

    private SplineContainer _splineContainer;
    private LineRenderer _lineRenderer;
    private Camera _cam;

    private Spline _spline;

    private float _splineLength;

    void Awake()
    {
        _splineContainer = GetComponent<SplineContainer>();
        _lineRenderer = GetComponent<LineRenderer>();
        _cam = Camera.main;

        _spline = _splineContainer.Spline;
        _splineLength = _spline.GetLength();

        DrawSpline();
    }

    void Update()
    {
        // Ensure line width remains constant regardless of camera distance
        _lineRenderer.startWidth = config.baseSplineWidth * _cam.orthographicSize / config.baseCameraOrthographicSize;

        // Find the length of the spline on the screen
        float _visualLength = _splineLength * config.baseCameraOrthographicSize / _cam.orthographicSize;

        // Calculate the number of dashes that should be displayed and scale texture accordingly
        float _numberOfDashes = (int)(_visualLength / config.baseSplineDashLength);
        _lineRenderer.textureScale = new Vector2(_numberOfDashes / _splineLength, 0);
    }

    private void DrawSpline()
    {
        _lineRenderer.positionCount = config.splineSegments;
        for (int i = 0; i < config.splineSegments; i++)
        {
            // Calculate normalized position along the spline
            float t = i / (float)(config.splineSegments - 1);
            Vector3 position = _spline.EvaluatePosition(t);

            // Set the position in the line renderer
            _lineRenderer.SetPosition(i, position);
        }
    }
}
