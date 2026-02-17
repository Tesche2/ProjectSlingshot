using UnityEngine;

[CreateAssetMenu(fileName = "GravityObjectConfig", menuName = "Config/Gravity Object Config")]
public class GravityObjectConfig : ScriptableObject
{
    [Header("Gravity")]
    [SerializeField] public float escapeAssistCoefficient = .1f;

    [Header("Camera")]
    [SerializeField] public float baseCameraOrthographicSize = 5;

    [Header("Outline")]
    [SerializeField] public int outlineSegments = 50;
    [SerializeField] public int minOutlineSegments = 3;
    [SerializeField] public float baseOutlineWidth = 0.1f;
    [SerializeField] public float baseOutlineDashLength = 1.5f;

    [Header("Trajectory Spline")]
    [SerializeField] public int splineSegments = 50;
    [SerializeField] public float baseSplineWidth = 0.1f;
    [SerializeField] public float baseSplineDashLength = 1.5f;
}
