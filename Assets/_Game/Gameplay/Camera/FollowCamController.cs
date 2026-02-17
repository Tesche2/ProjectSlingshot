using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public class FollowCamController : MonoBehaviour
{ 
    [SerializeField] private CameraConfig _config;
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private AnimationCurve _zoomCurve;

    private CinemachineCamera _cmCam;

    private void Awake()
    {
        _cmCam = GetComponent<CinemachineCamera>();
    }

    private void LateUpdate()
    {
        float playerSpeed = _playerRb.linearVelocity.magnitude;

        float targetZoom = _zoomCurve.Evaluate(playerSpeed);
        float currentZoom = _cmCam.Lens.OrthographicSize;

        float zoomSpeed = currentZoom > targetZoom ? _config.zoomInSpeed : _config.zoomOutSpeed;

        _cmCam.Lens.OrthographicSize = Mathf.Lerp(
            currentZoom,
            targetZoom,
            zoomSpeed * Time.deltaTime
        );
    }
}
