using UnityEngine;

public class DynamicScaler : MonoBehaviour
{
    [SerializeField] private float baseScale = 5f;

    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        transform.localScale = new Vector3(
            _cam.orthographicSize / baseScale,
            _cam.orthographicSize / baseScale,
            _cam.orthographicSize / baseScale
            );
    }
}
