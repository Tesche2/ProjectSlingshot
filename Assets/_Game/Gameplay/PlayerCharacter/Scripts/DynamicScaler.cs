using UnityEngine;

public class DynamicScaler : MonoBehaviour
{
    [SerializeField] private CameraConfig _camConfig;

    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        transform.localScale = new Vector3(
            _cam.orthographicSize / _camConfig.baseOrthographicSize,
            _cam.orthographicSize / _camConfig.baseOrthographicSize,
            _cam.orthographicSize / _camConfig.baseOrthographicSize
            );
    }
}
