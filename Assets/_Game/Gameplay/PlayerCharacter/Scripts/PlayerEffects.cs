using Unity.Cinemachine;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerController _controller;

    [Header("VFX")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ParticleSystem _thrusterParticles;
    [SerializeField] private GameObject _explosionParticlesObject;
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    private GameObject _currentThruster;

    private void OnEnable()
    {
        _controller.OnThrusterStart += PlayThrusterEffects;
        _controller.OnThrusterStop += StopThrusterEffects;
    }

    private void OnDisable()
    {
        _controller.OnThrusterStart -= PlayThrusterEffects;
        _controller.OnThrusterStop -= StopThrusterEffects;
    }

    private void PlayThrusterEffects()
    {
        _thrusterParticles.Play();
        _impulseSource.GenerateImpulse();
    }

    private void StopThrusterEffects()
    {
        _thrusterParticles.Stop(true);
    }

    public void Explode()
    {
        GameObject explosion = Instantiate(_explosionParticlesObject, transform);
        explosion.transform.SetParent(null);
        explosion.transform.localScale *= 3;
        explosion.SetActive(true);
        _spriteRenderer.enabled = false;
    }

    public void Respawn()
    {
        StopThrusterEffects();
        _spriteRenderer.enabled = true;

    }
}
