using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineBrain))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject cinemachineObject;
    [SerializeField] private float cameraMinZoom;
    [SerializeField] private float cameraMaxZoom;

    private CinemachineCamera cmCam;
    private CinemachinePositionComposer cmPosComp;

    private void Awake()
    {
        cmCam = cinemachineObject.GetComponent<CinemachineCamera>();
        cmPosComp = cinemachineObject.GetComponent<CinemachinePositionComposer>();
    }

    // Update is called once per frame
    void Update()
    {
        cmCam.Lens.OrthographicSize = Mathf.Clamp(player.getSpeed(), cameraMinZoom, cameraMaxZoom);
    }
}
