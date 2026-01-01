using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CinemachineImpulseSource))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _thrusterForce = 5f;
    [SerializeField] private float _maxThrusterSpeed = 10f;
    [SerializeField] private float _sidewaysCoefficient = 0.1f;
    [SerializeField] private float _backwardsCoefficient = 0.2f;
    [SerializeField] private float _torqueCoefficient = 0.001f;
    [SerializeField] private ParticleSystem _thrusterParticles;

    private PlayerInputActions _inputActions;
    private InputAction _moveAction;
    private InputAction _gravityAction;
    private Vector2 _currentInputVector;
    private bool _isThrusterActive;
    private CinemachineImpulseSource _impulseSource;

    private Rigidbody2D _rb;
    private List<GravityObject> _nearbyObjects = new();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _thrusterParticles.Stop(true);
        _inputActions = new PlayerInputActions();
        _moveAction = _inputActions.Gameplay.Move;
        _gravityAction = _inputActions.Gameplay.EnableGravity;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        _inputActions.Gameplay.Enable();

        _inputActions.Gameplay.Move.performed += ctx => _currentInputVector = ctx.ReadValue<Vector2>();
        _inputActions.Gameplay.Move.canceled += ctx => _currentInputVector = Vector2.zero;
    }

    private void OnDisable()
    {
        _inputActions.Gameplay.Disable();
    }    

    public void FixedUpdate()
    {
        if(_currentInputVector != Vector2.zero)
        {
            MovePlayer(_currentInputVector);
            RotatePlayer(_currentInputVector);
            _isThrusterActive = true;
        }
        else
        {
            _isThrusterActive = false;
        }

        if (_gravityAction.IsPressed())
        {
            _rb.AddForce(CalculateGravityInfluence());
        }
    }

    private void Update()
    {
        if (_isThrusterActive)
        {
            if (!_thrusterParticles.isPlaying)
            {
                _thrusterParticles.Play();
                _impulseSource.GenerateImpulse();
            }
        }
        else
        {
            if (_thrusterParticles.isPlaying) _thrusterParticles.Stop();
        }
    }

    private void MovePlayer(Vector2 direction)
    {
        _rb.AddForce(DefineThrusterForce(direction));
    }

    /**
     * 
     */
    private Vector2 DefineThrusterForce(Vector2 inputDirection)
    {
        Vector2 force = inputDirection * _thrusterForce;
        Vector2 vel = _rb.linearVelocity;

        // Get colinear and orthogonal components from the force in relation to the velocity.
        Vector2 colinear = Vector3.Project(force, vel);
        Vector2 orthogonal = force - colinear;

        // Forwards thrust is limited by current velocity, backwards thrust is strenghened by it, to facilitate stopping
        float colCoeff = Vector2.Dot(colinear, vel) > 0 ?  
            Mathf.Max(0, colinear.magnitude - vel.magnitude) : 
            Mathf.Max(1, vel.magnitude * _backwardsCoefficient);

        // Sideways thrust is strenghened by velocity, to facilitate steering
        float orthCoeff = Mathf.Max(1, vel.magnitude * _sidewaysCoefficient);

        return colinear * colCoeff + orthogonal * orthCoeff;
    }

    private void RotatePlayer(Vector2 inputDirection)
    {
        float angularDistance = Vector2.SignedAngle(this.transform.up, inputDirection);
        float aV = _rb.angularVelocity;
        float aD = angularDistance;

        float damping = ((aV >= 0 && aD <= 0) || (aV <= 0 && aD >=0)) ? 0.15f : 1f;
        _rb.AddTorque(angularDistance * _torqueCoefficient / damping);
    }

    /**
     *  Calculates the influence of nearby bodies stored in "nearbyObjects[]" using the inverse
     *  of the distance squared
     */
    private Vector2 CalculateGravityInfluence()
    {
        Vector2 totalForce = Vector2.zero ;
        Vector2 myPos = transform.position;

        foreach (var obj in _nearbyObjects)
        {
            Vector2 heading = obj.GetPosition() - myPos;
            float sqrDist = heading.sqrMagnitude;

            if (sqrDist < 0.001f) continue;
    
            totalForce += obj.gravityForce * heading.normalized / sqrDist;
        }

        return totalForce;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<GravityObject>(out var gravityObj))
        {
            if (!_nearbyObjects.Contains(gravityObj))
            {
                _nearbyObjects.Add(gravityObj);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.TryGetComponent<GravityObject>(out var gravityObj))
        {
            _nearbyObjects.Remove(gravityObj);
        }
    }

    public float getSpeed()
    {
        return _rb.linearVelocity.magnitude;
    }
}
