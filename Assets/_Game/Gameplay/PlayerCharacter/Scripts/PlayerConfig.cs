using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/Player Config")]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] public float thrusterForce = 5f;
    [SerializeField] public float sidewaysCoefficient = 0.1f;
    [SerializeField] public float backwardsCoefficient = 0.2f;
    [SerializeField] public float torqueCoefficient = 0.05f;

    [Header("Heat")]
    [SerializeField] public float heatThreshold = 10000f;
    [SerializeField] public float cooldownRate = 20.0f;
}
