using Unity.Cinemachine;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private float torqueCoefficient = 0.001f;

    [Header("Dependencies")]
    [SerializeField] private PlayerController controller;

    [Header("VFX")]
    [SerializeField] private ParticleSystem thrusterParticles;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        thrusterParticles.Stop();
    }

    private void OnEnable()
    {
        controller.OnThrusterStart += PlayThrusterEffects;
        controller.OnThrusterStop += StopThrusterEffects;
    }

    private void OnDisable()
    {
        controller.OnThrusterStart -= PlayThrusterEffects;
        controller.OnThrusterStop -= StopThrusterEffects;
    }

    private void PlayThrusterEffects()
    {
        thrusterParticles.Play();
        impulseSource.GenerateImpulse();
    }

    private void StopThrusterEffects()
    {
        thrusterParticles.Stop(true);
    }
}
