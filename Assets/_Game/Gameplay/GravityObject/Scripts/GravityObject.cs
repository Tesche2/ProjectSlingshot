using UnityEngine;

public class GravityObject : MonoBehaviour
{
    [SerializeField] private GravityObjectConfig config;
    [SerializeField] private float _gravityForce;

    private PlayerController _player;
    private Vector2 _currentPosition;
    private Vector2 _currentVelocity;

    // Keeps track of how many gravitational fields the player is currently inside of
    private static int _numberOfInfluencingFields = 0;

    private void Start()
    {
        _currentPosition = transform.position;
    }

    private void FixedUpdate()
    {
        _currentVelocity = (Vector2) transform.position - _currentPosition;
        _currentPosition = transform.position;

        if (_player != null && _player.isGravityActive)
        {
            ApplyGravityInfluence();
        }
    }

    private void ApplyGravityInfluence()
    {
        Vector2 playerPos = _player.transform.position;
        Vector2 myPos = this.transform.position;

        // Find direction and distance
        Vector2 heading = myPos - playerPos;
        float sqrDist = heading.sqrMagnitude;

        // Ignore tiny distances to avoid overblown forces
        if (sqrDist < 0.001f) return;

        // Calculate and apply force, proportional to the inverse of the square distance
        Vector2 force = _gravityForce * CalculateEscapeAssistForce(heading) * heading.normalized / sqrDist;
        _player.ApplyGravityForce(force);
    }

    // Tugs the player in if it's currently moving away from a planet, this helps make tighter turns around them
    private float CalculateEscapeAssistForce(Vector2 heading)
    {
        if (_numberOfInfluencingFields > 1) return 1f;

        Vector2 relativeVelocity = _player.GetVelocity() - _currentVelocity;

        // Get colinear and orthogonal components from the heading in relation to the relative velocity between object and player.
        float dotProduct = Vector2.Dot(relativeVelocity, -heading);

        if (dotProduct > 0)
            return 1f + (dotProduct * config.escapeAssistCoefficient);
        else return 1f;
    }

    private void OnTriggerEnter2D(Collider2D playerCollider)
    {
        _player = playerCollider.gameObject.GetComponent<PlayerController>();
        _numberOfInfluencingFields += 1;
        Debug.Log(_numberOfInfluencingFields);
    }

    private void OnTriggerExit2D(Collider2D playerCollider)
    {
        _numberOfInfluencingFields -= 1;
        Debug.Log(_numberOfInfluencingFields);
        _player = null;
    }

}
