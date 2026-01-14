using UnityEngine;

[CreateAssetMenu(fileName = "CameraConfig", menuName = "Config/Camera Config")]
public class CameraConfig : ScriptableObject
{
    [Header("Overview Camera")]
    [SerializeField] public float oscillationFrequency = 1.0f;

    [Header("Follow Camera")]
    [SerializeField] public AnimationCurve zoomCurve;
    [SerializeField] public float zoomInSpeed = 0.5f;
    [SerializeField] public float zoomOutSpeed = 5f;
}
