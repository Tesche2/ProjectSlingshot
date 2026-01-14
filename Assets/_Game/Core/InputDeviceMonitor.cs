using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public enum DeviceType
{
    KeyboardMouse,
    Gamepad,
    Touch
}

public class InputDeviceMonitor : MonoBehaviour
{
    #region Singleton
    public static InputDeviceMonitor Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public System.Action<DeviceType> OnDeviceChanged;
    public DeviceType currentDevice {  get; private set; } = DeviceType.KeyboardMouse;

    private void OnEnable()
    {
        InputSystem.onEvent += OnInputEvent;
    }

    private void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        // Ignore irrelevant events or stick input (in case of connected device with drift)
        if (device == null) return;
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) return;

        // Define input type
        DeviceType detectedDevice = currentDevice;

        if(device is Gamepad)
        {
            detectedDevice = DeviceType.Gamepad;
        }
        else if (device is Keyboard || device is Mouse)
        {
            detectedDevice = DeviceType.KeyboardMouse;
        }
        else if (device is Touchscreen)
        {
            detectedDevice = DeviceType.Touch;
        }

        // Emit event if device changed
        if (detectedDevice != currentDevice)
        {
            currentDevice = detectedDevice;
            Debug.Log($"Device Changed to {currentDevice}");
            OnDeviceChanged?.Invoke(detectedDevice);
        }

    }
}
