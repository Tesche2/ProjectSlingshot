using UnityEngine;

[CreateAssetMenu(fileName = "GravityObjectConfig", menuName = "Config/Gravity Object Config")]
public class GravityObjectConfig : ScriptableObject
{
    [Header("Gravity")]
    public float escapeAssistCoefficient = .1f;

    [Header("Outline")]
    public int outlineSegments = 50;
    public int minOutlineSegments = 3;
    public float baseOutlineWidth = 0.1f;
    public float baseOutlineDashLength = 1.5f;

    [Header("Trajectory Spline")]
    public int splineSegments = 50;
    public float baseSplineWidth = 0.1f;
    public float baseSplineDashLength = 1.5f;
}
