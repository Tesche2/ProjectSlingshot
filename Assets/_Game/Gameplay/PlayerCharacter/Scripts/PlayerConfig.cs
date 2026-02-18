using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/Player Config")]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    public float thrusterForce = 5f;
    public float sidewaysCoefficient = 0.1f;
    public float backwardsCoefficient = 0.2f;
    public float torqueCoefficient = 0.05f;

    [Header("Heat")]
    public float heatThreshold = 10000f;
    public float cooldownRate = 20.0f;
    public float gaugeOpacityThreshold = .2f;
}
