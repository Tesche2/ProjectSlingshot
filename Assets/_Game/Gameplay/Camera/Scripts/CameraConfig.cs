using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "Config/Camera Config")]
public class CameraConfig : ScriptableObject
{
    [Header("Overview Camera")]
    public float oscillationFrequency = 1.0f;

    [Header("Follow Camera")]
    public float zoomInSpeed = 0.5f;
    public float zoomOutSpeed = 5f;

    [Header("Misc")]
    public float baseOrthographicSize = 5f;
}
