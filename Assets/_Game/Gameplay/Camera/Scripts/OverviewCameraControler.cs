using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public class OverviewCameraControler : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private CameraConfig _config;

    [Header("Dependencies")]
    [SerializeField] private RectTransform _bounds;

    private CinemachineSplineDolly _dolly;
    private CinemachineCamera _cam;
    private Rect _rect;
    private Spline _spline;
    private float time;

    void Awake()
    {
        _dolly = GetComponent<CinemachineSplineDolly>();
        _cam = GetComponent<CinemachineCamera>();
        _spline = _dolly.Spline.Spline;
        _rect = _bounds.rect;

        float camRatio = Camera.main.aspect;
        float boundsRatio = _rect.width / _rect.height;

        bool isBoundsWider = boundsRatio > camRatio;
        Debug.Log("Bounds Position: " + _bounds.position);

        if (isBoundsWider)
        {
            // Set zoom to match the bounds height
            _cam.Lens.OrthographicSize = _rect.height / 2;

            // Get camera width in world units
            float camWorldWidth = _cam.Lens.OrthographicSize * 2 * camRatio;

            // Set spline to cover the remaining width
            float remainingWidth = _rect.width - camWorldWidth;
            SetKnotPosition(0, new(_bounds.position.x - remainingWidth / 2, _bounds.position.y, -10));
            SetKnotPosition(1, new(_bounds.position.x + remainingWidth / 2, _bounds.position.y, -10));
        }
        else
        {
            // Set zoom to match the bounds width
            _cam.Lens.OrthographicSize = _rect.height / 2 / camRatio * boundsRatio;

            // Get camera height in world units
            float camWorldHeight = _cam.Lens.OrthographicSize * 2;

            // Set spline to cover the remaining height
            float remainingHeight = _rect.height - camWorldHeight;
            SetKnotPosition(0, new(_bounds.position.x, _bounds.position.y - remainingHeight / 2, -10));
            SetKnotPosition(1, new(_bounds.position.x, _bounds.position.y + remainingHeight / 2, -10));
        }
    }

    // Update is called once per frame
    void Update()
    {
        _dolly.CameraPosition = Mathf.Cos(time * _config.oscillationFrequency) / 2 + .5f;
        time += Time.deltaTime;
    }

    private void SetKnotPosition(int index, Vector3 worldPos)
    {
        _spline.SetKnot(index, new BezierKnot(_dolly.Spline.transform.InverseTransformPoint(worldPos)));
    }
}
