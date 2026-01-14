using TMPro;
using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance;

    [SerializeField] GameObject MobileControls;
    [SerializeField] GameObject GameMenu;
    [SerializeField] GameObject OverviewMessage;
    [SerializeField] GameObject Countdown;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        LevelManager.Instance.OnOverviewStart += EnableOverviewMessage;
        LevelManager.Instance.OnCountdownStart += EnableCountdown;
        LevelManager.Instance.OnGameplayStart += GameStarted;
        LevelManager.Instance.OnMenuStart += EnableGameMenu;

        LevelManager.Instance.OnCountdownMessage += DefineCountdownMessage;
        InputDeviceMonitor.Instance.OnDeviceChanged += InputDeviceChanged;
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnOverviewStart -= EnableOverviewMessage;
        LevelManager.Instance.OnCountdownStart -= EnableCountdown;
        LevelManager.Instance.OnGameplayStart -= GameStarted;
        LevelManager.Instance.OnMenuStart -= EnableGameMenu;

        LevelManager.Instance.OnCountdownMessage -= DefineCountdownMessage;
        InputDeviceMonitor.Instance.OnDeviceChanged -= InputDeviceChanged;
    }

    private void InputDeviceChanged(DeviceType device)
    {
        DefineOverviewMessage(device);

        LevelState state = LevelManager.Instance.currentState;
        if (device == DeviceType.Touch && (state == LevelState.Countdown || state == LevelState.Gameplay)) MobileControls.SetActive(true);
        else MobileControls.SetActive(false);
    }

    private void GameStarted()
    {
        DisableAllUI();
        CheckForTouchOverlay();
    }

    private void EnableCountdown()
    {
        DisableAllUI();
        CheckForTouchOverlay();

        Countdown.SetActive(true);
    }

    private void EnableOverviewMessage()
    {
        DisableAllUI();

        DefineOverviewMessage(InputDeviceMonitor.Instance.currentDevice);
        OverviewMessage.SetActive(true);
    }

    private void EnableGameMenu()
    {
        DisableAllUI();

        GameMenu.SetActive(true);
    }

    private void DefineOverviewMessage(DeviceType device)
    {
        TextMeshProUGUI overviewTMP = OverviewMessage.GetComponent<TextMeshProUGUI>();

        switch(device)
        {
            case DeviceType.Gamepad:
                overviewTMP.text = "Press A to start";
                break;

            case DeviceType.KeyboardMouse:
                overviewTMP.text = "Press any key to start";
                break;

            case DeviceType.Touch:
                overviewTMP.text = "Tap the screen to start";
                break;
        }
    }

    private void DefineCountdownMessage(string message)
    {
        TextMeshProUGUI countdownTMP = Countdown.GetComponent<TextMeshProUGUI>();
        countdownTMP.text = message;
    }

    private void CheckForTouchOverlay()
    {
        if (InputDeviceMonitor.Instance.currentDevice == DeviceType.Touch) MobileControls.SetActive(true);
        else MobileControls.SetActive(false);
    }

    private void DisableAllUI()
    {
        MobileControls.SetActive(false);
        OverviewMessage.SetActive(false);
        Countdown.SetActive(false);
        GameMenu.SetActive(false);
    }
}
