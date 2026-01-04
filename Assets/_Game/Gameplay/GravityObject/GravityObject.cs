using UnityEngine;
using UnityEngine.Splines;

public class GravityObject : MonoBehaviour
{
    [SerializeField] private float _gravityForce;

    private PlayerController _player;

    private void FixedUpdate()
    {

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
        Vector2 force = _gravityForce * heading.normalized / sqrDist;
        _player.ApplyGravityForce(force);
    }

    private void OnTriggerEnter2D(Collider2D playerCollider)
    {
        _player = playerCollider.gameObject.GetComponent<PlayerController>();
    }

    private void OnTriggerExit2D(Collider2D playerCollider)
    {
        _player = null;
    }

}
