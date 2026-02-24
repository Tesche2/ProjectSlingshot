using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SplineContainer))]
public class SplineRenderer : MonoBehaviour
{
    [SerializeField] private GravityObjectConfig _config;
    [SerializeField] private CameraConfig _camConfig;
    
    private SplineContainer _splineContainer;
    private LineRenderer _lineRenderer;
    private Camera _cam;
    private float _splineLength;
    private Material _material;

    private Spline _spline;

    void Awake()
    {
        _splineContainer = GetComponent<SplineContainer>();
        _lineRenderer = GetComponent<LineRenderer>();
        _cam = Camera.main;

        _spline = _splineContainer.Spline;
        _splineLength = _spline.GetLength();
        _material = _lineRenderer.material;
    }

    private void Start()
    {
        DrawSpline();
    }

    void LateUpdate()
    {
        // Ensure line width remains constant regardless of camera distance
        _lineRenderer.startWidth = _config.baseSplineWidth * _cam.orthographicSize;

        // Find the length of the spline on the screen, based on camera distance
        float visualLength = _splineLength/ _cam.orthographicSize;

        // Calculate the length of dashes based on camera distance and scale texture accordingly
        float numberOfDashes = (int) (visualLength / _config.baseSplineDashLength);
        _lineRenderer.textureScale = new Vector2(numberOfDashes / _splineLength, 1);

        // Offset texture to ensure smooth scaling in regions closer to the camera
        Vector3 cameraRelativeToSpline = _splineContainer.transform.InverseTransformPoint(_cam.transform.position);
        SplineUtility.GetNearestPoint(_spline, cameraRelativeToSpline, out _, out float t);
        _material.SetTextureOffset("_MainTex", new Vector2(numberOfDashes * (1 - t), 0));
    }

    private void OnValidate()
    {
        if (_splineContainer == null) _splineContainer = GetComponent<SplineContainer>();
        if (_lineRenderer == null) _lineRenderer = GetComponent<LineRenderer>();
        if (_cam == null) _cam = Camera.main;

        _spline = _splineContainer.Spline;

        DrawSpline();
    }

    private void DrawSpline()
    {
        _lineRenderer.positionCount = _config.splineSegments;
        for (int i = 0; i < _config.splineSegments; i++)
        {
            // Calculate normalized position along the spline
            float t = i / (float)(_config.splineSegments);
            Vector3 position = _spline.EvaluatePosition(t);

            // Set the position in the line renderer
            _lineRenderer.SetPosition(i, position);
        }
    }
}
