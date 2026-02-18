using Unity.VisualScripting;
using UnityEngine;

public class HeatHandler : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;

    public float CurrentTemperature { get; private set;}
    public bool isInHeatField = false;

    public System.Action onOverheat;

    private bool _isOverheated = false;

    void FixedUpdate()
    {
        if(!isInHeatField)
            CurrentTemperature = Mathf.Max(0, CurrentTemperature - _config.cooldownRate);
    }

    public void HeatUp(float temperature)
    {
        CurrentTemperature += temperature;

        if (CurrentTemperature >= _config.heatThreshold && !_isOverheated)
        {
            onOverheat.Invoke();
            _isOverheated = true;
        }
    }

    public void ResetTemperature()
    {
        CurrentTemperature = 0f;
        _isOverheated = false;
    }
}
