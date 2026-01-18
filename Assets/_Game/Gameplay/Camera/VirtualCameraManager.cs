using Unity.Cinemachine;
using UnityEngine;

public class VirtualCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera overviewCamera;
    [SerializeField] private CinemachineCamera followCamera;

    private void Awake()
    {
        ActivateOverview();
    }

    private void OnEnable()
    {
        LevelManager.Instance.OnZoomInStart += ActivateFollow;
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnZoomInStart -= ActivateFollow;
    }

    private void ActivateOverview()
    {
        overviewCamera.Priority = 1;
        followCamera.Priority = 0;
    }

    private void ActivateFollow()
    {
        overviewCamera.Priority = 0;
        followCamera.Priority = 1;
        Debug.Log("Changing to Follow Camera");
    }
}
