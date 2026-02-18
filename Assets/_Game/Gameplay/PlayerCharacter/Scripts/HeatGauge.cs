using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HeatGauge : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;
    [SerializeField] private HeatHandler _heatHandler;
    [SerializeField] private CanvasGroup _canvasGroup;

    private Slider _slider;

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    void Start()
    {
        UpdateGaugeSlider();
        UpdateSliderOpacity();
    }

    void Update()
    {
        UpdateGaugeSlider();
        UpdateSliderOpacity();
    }

    private void UpdateGaugeSlider()
    {
        _slider.value = _heatHandler.CurrentTemperature / _config.heatThreshold;
    }

    private void UpdateSliderOpacity()
    {
        _canvasGroup.alpha = _heatHandler.CurrentTemperature / _config.heatThreshold / _config.gaugeOpacityThreshold;
    }
}
