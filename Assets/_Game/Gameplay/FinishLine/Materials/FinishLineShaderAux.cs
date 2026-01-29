using UnityEngine;

[ExecuteInEditMode]
public class FinishLineShaderAux : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private MaterialPropertyBlock _propBlock;

    private static readonly int HeightProp = Shader.PropertyToID("_ObjectHeight");
    private static readonly int WidthProp = Shader.PropertyToID("_ObjectWidth");

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateShaderProperties();
    }

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        UpdateShaderProperties();
    }

    private void OnValidate()
    {
        if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
        if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

        UpdateShaderProperties();
    }
#endif

    private void UpdateShaderProperties()
    {
        if (_renderer == null) return;

        _renderer.GetPropertyBlock(_propBlock);

        float height = _renderer.size.y * transform.localScale.y;
        _propBlock.SetFloat(HeightProp, height);
        float width = _renderer.size.x * transform.localScale.x;
        _propBlock.SetFloat(WidthProp, width);

        _renderer.SetPropertyBlock(_propBlock);
    }
}
