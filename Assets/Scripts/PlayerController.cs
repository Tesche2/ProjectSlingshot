using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float thrusterForce = 5f;
    [SerializeField] private float maxThrusterSpeed = 10f;
    [SerializeField] private float sidewaysCoefficient = 0.1f;
    [SerializeField] private float backwardsCoefficient = 0.2f;
    [SerializeField] private InputActionReference moveActionRef;
    [SerializeField] private InputActionReference gravityActionRef;
    [SerializeField] private float inputDeadZone = 0.5f;
    [SerializeField] private float torqueCoefficient = 0.001f;
    [SerializeField] private ParticleSystem thrusterParticles;

    private Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction gravityAction;
    private List<GravityObject> nearbyObjects = new();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction = moveActionRef.action;
        gravityAction = gravityActionRef.action;
        thrusterParticles.Stop(true);
    }

    private void OnEnable()
    {
        moveAction.Enable();
        gravityAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        gravityAction.Disable();
    }

    public void FixedUpdate()
    {
        Vector2 movementInput = moveAction.ReadValue<Vector2>();
        if(movementInput.magnitude > inputDeadZone)
        {
            MovePlayer(movementInput);
            RotatePlayer(movementInput);
        }
        if(gravityAction.IsPressed())
        {
            rb.AddForce(CalculateGravityInfluence());
            Debug.DrawRay(this.transform.position, CalculateGravityInfluence());
        }

        if(moveAction.WasPressedThisFrame())
        {
            Debug.Log("Pressed!");
            thrusterParticles.Play(true);
        }

        if(moveAction.WasReleasedThisFrame())
        {
            Debug.Log("Released!");
            thrusterParticles.Stop(true); 
        }
    }

    private void MovePlayer(Vector2 direction)
    {
        rb.AddForce(DefineThrusterForce(direction));
    }

    /**
     * 
     */
    private Vector2 DefineThrusterForce(Vector2 inputDirection)
    {
        Vector2 force = inputDirection * thrusterForce;
        Vector2 vel = rb.linearVelocity;

        // Get colinear and orthogonal components from the force in relation to the velocity.
        Vector2 colComp = Vector3.Project(force, vel);
        Vector2 orthComp = force - colComp;

        // Forwards thrust is limited by current velocity, backwards thrust is strenghened by it, to facilitate stopping
        float colCoeff = Vector2.Dot(colComp, vel) > 0 ?  
            Mathf.Max(0, colComp.magnitude - vel.magnitude) : 
            Mathf.Max(1, vel.magnitude * backwardsCoefficient);

        // Sideways thrust is strenghened by velocity, to facilitate steering
        float orthCoeff = Mathf.Max(1, vel.magnitude * sidewaysCoefficient);

        return colComp * colCoeff + orthComp * orthCoeff;
    }

    private void RotatePlayer(Vector2 inputDirection)
    {
        float angularDistance = Vector2.SignedAngle(this.transform.up, inputDirection);
        float aV = rb.angularVelocity;
        float aD = angularDistance;

        float damping = ((aV >= 0 && aD <= 0) || (aV <= 0 && aD >=0)) ? 0.15f : 1f;
        rb.AddTorque(angularDistance * torqueCoefficient / damping);
    }

    /**
     *  Calculates the influence of nearby bodies stored in "nearbyObjects[]" using the inverse
     *  of the distance squared
     */
    private Vector2 CalculateGravityInfluence()
    {
        Vector2 influence = new();

        foreach (var obj in nearbyObjects)
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
        nearbyObjects.Add(col.gameObject.GetComponent<GravityObject>());
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        nearbyObjects.Remove(col.gameObject.GetComponent<GravityObject>());  
    }

    public float getSpeed()
    {
        return rb.linearVelocity.magnitude;
    }
}
