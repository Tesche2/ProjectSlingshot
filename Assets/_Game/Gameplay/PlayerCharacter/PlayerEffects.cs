using Unity.Cinemachine;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
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

    public void StopThrusterEffects()
    {
        thrusterParticles.Stop(true);
    }
}
