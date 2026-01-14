using Unity.Cinemachine;
using UnityEngine;

public class FollowCamController : MonoBehaviour
{ 
    [SerializeField] private CameraConfig config;
    [SerializeField] private Rigidbody2D playerRb;

    private CinemachineCamera cmCam;

    private void Awake()
    {
        cmCam = GetComponent<CinemachineCamera>();
    }

    private void LateUpdate()
    {
        float playerSpeed = playerRb.linearVelocity.magnitude;

        float targetZoom = config.zoomCurve.Evaluate(playerSpeed);
        float currentZoom = cmCam.Lens.OrthographicSize;

        float zoomSpeed = currentZoom > targetZoom ? config.zoomInSpeed : config.zoomOutSpeed;

        cmCam.Lens.OrthographicSize = Mathf.Lerp(
            currentZoom,
            targetZoom,
            zoomSpeed * Time.deltaTime
        );
    }
}
