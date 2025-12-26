using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class VCamController : MonoBehaviour
{
    [SerializeField] private AnimationCurve zoomCurve;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float zoomInSpeed = 0.5f;
    [SerializeField] private float zoomOutSpeed = 5f;

    private CinemachineCamera cmCam;

    private void Awake()
    {
        cmCam = GetComponent<CinemachineCamera>();
    }

    private void LateUpdate()
    {
        float playerSpeed = playerRb.linearVelocity.magnitude;

        float targetZoom = zoomCurve.Evaluate(playerSpeed);
        float currentZoom = cmCam.Lens.OrthographicSize;

        float zoomSpeed = currentZoom > targetZoom ? zoomInSpeed : zoomOutSpeed;

        cmCam.Lens.OrthographicSize = Mathf.Lerp(
            currentZoom,
            targetZoom,
            zoomSpeed * Time.deltaTime
        );
    }
}
