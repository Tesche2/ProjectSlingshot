using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AtmosphereStyle : MonoBehaviour
{

    [SerializeField] private Color atmosphereColor = Color.white;
    [SerializeField] private float speedConfig = 10f;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ApplyColor();
    }

    private void OnValidate()
    {
        if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();

        ApplyColor();
    }

    void ApplyColor()
    {
        MaterialPropertyBlock mpb = new();

        _renderer.GetPropertyBlock(mpb);

        mpb.SetColor("_AtmosphereColor", atmosphereColor);
        mpb.SetFloat("_SpeedConfig", speedConfig);

        _renderer.SetPropertyBlock(mpb);
    }
}
