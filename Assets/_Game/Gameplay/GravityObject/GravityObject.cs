using UnityEngine;
using UnityEngine.Splines;

public class GravityObject : MonoBehaviour
{
    public float gravityForce;

    public Vector2 GetPosition()
    {
        return this.transform.position;
    }
}
