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
            LevelManager.Instance.OnCountdownStart += HandleReset;

            _heatHandler.onOverheat += KillPlayer;
    }

    private void OnDisable()
    {
            LevelManager.Instance.OnCountdownStart -= HandleReset;

            _heatHandler.onOverheat -= KillPlayer;
    }

    private void HandleReset()
    {
        _controller.transform.position = _initialPos;
        _controller.transform.rotation = _initialRot;
        _controller.ResetPhysics();
        if(_effects) _effects.StopThrusterEffects();
        _heatHandler.ResetTemperature();
    }

    private void KillPlayer()
    {
        Debug.Log("PLAYER SPLODED");
        _controller.ResetPhysics();
        if (_effects) _effects.StopThrusterEffects();
        LevelManager.Instance.SetState(LevelState.PlayerDead);
    }
}
