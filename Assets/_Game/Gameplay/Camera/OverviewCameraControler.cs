using Unity.Cinemachine;
using UnityEngine;

public class OverviewCameraControler : MonoBehaviour
{
    [SerializeField] private CameraConfig config;

    private CinemachineSplineDolly _dolly;
    private float time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _dolly = GetComponent<CinemachineSplineDolly>();
    }

    // Update is called once per frame
    void Update()
    {
        _dolly.CameraPosition = Mathf.Cos(time * config.oscillationFrequency) / 2 + .5f;
        time += Time.deltaTime;
    }
}
