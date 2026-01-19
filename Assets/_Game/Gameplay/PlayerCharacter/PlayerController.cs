using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CinemachineImpulseSource))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerConfig config;

    public event Action OnThrusterStart;
    public event Action OnThrusterStop;
    public event Action<Vector3, Vector3> OnFinishLineCross;

    private Vector2 _currentInputVector;
    private Rigidbody2D _rb;
    public Vector3 PreviousFramePos { get; private set; }

    [HideInInspector] public bool isGravityActive = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        GameInput.GameplayActions gameplayActions = GlobalInputManager.Instance.InputActions.Gameplay;

        gameplayActions.Move.performed += ctx => _currentInputVector = ctx.ReadValue<Vector2>();
        gameplayActions.Move.started += _ => HandleThrusterStarted();
        gameplayActions.Move.canceled += _ => HandleThrusterCanceled();

        gameplayActions.EnableGravity.performed += _ => isGravityActive = true;
        gameplayActions.EnableGravity.canceled += _ => isGravityActive = false;

        gameplayActions.Enable();
    }

    private void OnDisable()
    {
        GameInput.GameplayActions gameplayActions = GlobalInputManager.Instance.InputActions.Gameplay;

        gameplayActions.Move.performed -= ctx => _currentInputVector = ctx.ReadValue<Vector2>();
        gameplayActions.Move.started -= _ => HandleThrusterStarted();
        gameplayActions.Move.canceled -= _ => HandleThrusterCanceled();

        gameplayActions.EnableGravity.performed -= _ => isGravityActive = true;
        gameplayActions.EnableGravity.canceled -= _ => isGravityActive = false;

        gameplayActions.Disable();
    }    

    public void FixedUpdate()
    {
        if(_currentInputVector != Vector2.zero)
        {
            MovePlayer(_currentInputVector);
            RotatePlayer(_currentInputVector);
        } else if (_rb.linearVelocity.sqrMagnitude >= 10)
        {
            RotatePlayer(_rb.linearVelocity);
        }

        PreviousFramePos = transform.position;
    }

    private void MovePlayer(Vector2 direction)
    {
        _rb.AddForce(DefineThrusterForce(direction));
    }

    private Vector2 DefineThrusterForce(Vector2 inputDirection)
    {
        Vector2 force = inputDirection * config.thrusterForce;
        Vector2 vel = _rb.linearVelocity;

        // Get colinear and orthogonal components from the force in relation to the velocity.
        Vector2 colinear = Vector3.Project(force, vel);
        Vector2 orthogonal = force - colinear;

        // Forwards thrust is limited by current velocity, backwards thrust is strenghened by it, to facilitate stopping
        float colCoeff = Vector2.Dot(colinear, vel) > 0 ?  
            Mathf.Max(0, colinear.magnitude - vel.magnitude) : 
            Mathf.Max(1, vel.magnitude * config.backwardsCoefficient);

        // Sideways thrust is strenghened by velocity, to facilitate steering
        float orthCoeff = Mathf.Max(1, vel.magnitude * config.sidewaysCoefficient);

        return colinear * colCoeff + orthogonal * orthCoeff;
    }

    private void RotatePlayer(Vector2 inputDirection)
    {
        float angularDistance = Vector2.SignedAngle(this.transform.up, inputDirection);
        float aV = _rb.angularVelocity;
        float aD = angularDistance;

        float damping = ((aV >= 0 && aD <= 0) || (aV <= 0 && aD >=0)) ? 0.15f : 1f;
        _rb.AddTorque(angularDistance * config.torqueCoefficient / damping);
    }

    private void HandleThrusterStarted()
    {
        OnThrusterStart.Invoke();
    }

    private void HandleThrusterCanceled()
    {
        _currentInputVector = Vector2.zero;
        OnThrusterStop.Invoke();
    }

    public void ApplyGravityForce(Vector2 force)
    {
        _rb.AddForce(force);
    }

    public float getSpeed()
    {
        return _rb.linearVelocity.magnitude;
    }

    public void ResetPhysics()
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        _currentInputVector = Vector2.zero;
        isGravityActive = false;
    }
}
