using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _thrusterForce = 5f;
    [SerializeField] private float _maxThrusterSpeed = 10f;
    [SerializeField] private float _sidewaysCoefficient = 0.1f;
    [SerializeField] private float _backwardsCoefficient = 0.2f;
    [SerializeField] private float _inputDeadZone = 0.5f;
    [SerializeField] private float _torqueCoefficient = 0.001f;
    [SerializeField] private ParticleSystem _thrusterParticles;

    private Rigidbody2D _rb;
    private PlayerInputActions _inputActions;
    private InputAction _moveAction;
    private InputAction _gravityAction;
    private List<GravityObject> _nearbyObjects = new();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _thrusterParticles.Stop(true);
        _inputActions = new PlayerInputActions();
        _moveAction = _inputActions.Gameplay.Move;
        _gravityAction = _inputActions.Gameplay.EnableGravity;
    }

    private void OnEnable()
    {
        _inputActions.Gameplay.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Gameplay.Disable();
    }

    public void FixedUpdate()
    {
        Vector2 movementInput = _moveAction.ReadValue<Vector2>();
        if(movementInput.magnitude > _inputDeadZone)
        {
            MovePlayer(movementInput);
            RotatePlayer(movementInput);
        }
        if(_gravityAction.IsPressed())
        {
            _rb.AddForce(CalculateGravityInfluence());
            Debug.DrawRay(this.transform.position, CalculateGravityInfluence());
        }

        if (_moveAction.WasPressedThisFrame())
        {
            Debug.Log("Pressed!");
            _thrusterParticles.Play(true);
        }

        if(_moveAction.WasReleasedThisFrame())
        {
            Debug.Log("Released!");
            _thrusterParticles.Stop(true); 
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
        Vector2 influence = new();

        foreach (var obj in _nearbyObjects)
        {
            Vector2 heading = obj.GetPosition() - (Vector2) this.transform.position;
            influence += obj.gravityForce * heading.normalized / Mathf.Pow(heading.magnitude, 2);
        }

        return influence;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Enter");
        _nearbyObjects.Add(col.gameObject.GetComponent<GravityObject>());
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        _nearbyObjects.Remove(col.gameObject.GetComponent<GravityObject>());  
    }

    public float getSpeed()
    {
        return _rb.linearVelocity.magnitude;
    }
}
