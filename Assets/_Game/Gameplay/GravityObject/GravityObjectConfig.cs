using UnityEngine;

[CreateAssetMenu(fileName = "GravityObjectConfig", menuName = "Config/Gravity Object Config")]
public class GravityObjectConfig : ScriptableObject
{
    [Header("Outline")]
    [SerializeField] public int segments = 50;
    [SerializeField] public int minSegments = 3;
    [SerializeField] public float baseWidth = 0.1f;
    [SerializeField] public float baseDashLength = 1.5f;
}
