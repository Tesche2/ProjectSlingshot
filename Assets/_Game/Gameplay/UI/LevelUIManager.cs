using System;
using TMPro;
using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance;

    [SerializeField] GameObject MobileControls;
    [SerializeField] GameObject GameMenu;
    [SerializeField] GameObject OverviewMessage;
    [SerializeField] GameObject Countdown;
    [SerializeField] GameObject Timer;

    TextMeshProUGUI _timerTMP;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

            DisableAllUI();

        _timerTMP = Timer.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        LevelManager.Instance.OnOverviewStart += EnableOverviewMessage;
        LevelManager.Instance.OnZoomInStart += DisableAllUI;
        LevelManager.Instance.OnCountdownStart += EnableCountdown;
        LevelManager.Instance.OnGameplayStart += GameStarted;
        LevelManager.Instance.OnMenuStart += EnableGameMenu;

        LevelManager.Instance.OnCountdownMessage += DefineCountdownMessage;
        InputDeviceMonitor.Instance.OnDeviceChanged += InputDeviceChanged;
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnOverviewStart -= EnableOverviewMessage;
        LevelManager.Instance.OnZoomInStart -= DisableAllUI;
        LevelManager.Instance.OnCountdownStart -= EnableCountdown;
        LevelManager.Instance.OnGameplayStart -= GameStarted;
        LevelManager.Instance.OnMenuStart -= EnableGameMenu;

        LevelManager.Instance.OnCountdownMessage -= DefineCountdownMessage;
        InputDeviceMonitor.Instance.OnDeviceChanged -= InputDeviceChanged;
    }

    private void Update()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(LevelManager.Instance.timeValue);

        string formattedTime = timeSpan.ToString(@"mm\:ss\.fff");

        _timerTMP.text = formattedTime;
    }

    private void InputDeviceChanged(DeviceType device)
    {
        DefineOverviewMessage(device);

        LevelState state = LevelManager.Instance.currentState;

        bool isTouch = device == DeviceType.Touch;
        bool isGameRunning = state == LevelState.Countdown || state == LevelState.Gameplay || state == LevelState.Finished;

        if (isTouch && isGameRunning) MobileControls.SetActive(true);
        else MobileControls.SetActive(false);
    }

    private void GameStarted()
    {
        DisableAllUI();
        CheckForTouchOverlay();

        Timer.SetActive(true);
    }

    private void EnableCountdown()
    {
        DisableAllUI();
        CheckForTouchOverlay();

        Debug.Log("Display Countdown");

        Timer.SetActive(true);
        Countdown.SetActive(true);
    }

    private void EnableOverviewMessage()
    {
        DisableAllUI();

        Debug.Log("Display Overview");

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
        if (InputDeviceMonitor.Instance.currentDevice == DeviceType.Touch)
        {
            if (!MobileControls.activeSelf) MobileControls.SetActive(true);
        }
        else MobileControls.SetActive(false);
    }

    private void DisableAllUI()
    {
        OverviewMessage.SetActive(false);
        Countdown.SetActive(false);
        GameMenu.SetActive(false);
        Timer.SetActive(false);
    }
}
