using UnityEngine;

public class PlayerLevelHandler : MonoBehaviour
{

    [SerializeField] private PlayerController _controller;
    [SerializeField] private PlayerEffects _effects;
    [SerializeField] private HeatHandler _heatHandler;

    private Vector3 _initialPos;
    private Quaternion _initialRot;

    private void Awake()
    {
        _initialPos = _controller.transform.position;
        _initialRot = _controller.transform.rotation;
    }

    private void OnEnable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnCountdownStart += HandleReset;
        }
    }

    private void OnDisable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnCountdownStart -= HandleReset;
        }
    }

    private void HandleReset()
    {
        _controller.transform.position = _initialPos;
        _controller.transform.rotation = _initialRot;
        _controller.ResetPhysics();
        if(_effects) _effects.StopThrusterEffects();
    }
}
