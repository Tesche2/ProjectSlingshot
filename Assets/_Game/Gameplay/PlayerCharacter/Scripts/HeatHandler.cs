using Unity.VisualScripting;
using UnityEngine;

public class HeatHandler : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;

    private float _currentTemperature;
    public bool isInHeatField = false;

    void FixedUpdate()
    {
        if(!isInHeatField)
            _currentTemperature = Mathf.Max(0, _currentTemperature - _config.cooldownRate);

        Debug.Log(_currentTemperature);
        
        if (_currentTemperature > _config.heatThreshold)
            Debug.Log("OVERHEATING");
    }

    public void HeatUp(float temperature)
    {
        Debug.Log($"Heat up {temperature}");
        _currentTemperature += temperature;
    }
}
