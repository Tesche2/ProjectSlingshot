using Unity.Cinemachine;
using UnityEngine;

public class VirtualCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera OverviewCamera;
    [SerializeField] private CinemachineCamera followCamera;

    private void Awake()
    {
        OverviewCamera.Prioritize();
    }

    private void OnEnable()
    {
        LevelManager.Instance.OnCountdownStart += PrioritizeFollowCamera;
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnCountdownStart -= PrioritizeFollowCamera;
    }

    private void PrioritizeFollowCamera()
    {
        followCamera.Prioritize();
    }
}
