using NUnit.Framework;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance;

    private readonly static WaitForSeconds _waitForSeconds1 = new(1);

    [Header("UI Elements")]
    [SerializeField] GameObject _ui_mobileControls;
    [SerializeField] GameObject _ui_gameMenu;
    [SerializeField] GameObject _ui_overviewMessage;
    [SerializeField] GameObject _ui_countdown;
    [SerializeField] GameObject _ui_timer;
    [SerializeField] GameObject _ui_endMenu;

    [Header("Dependencies")]
    [SerializeField] TimerManager _timerManager;

    TextMeshProUGUI _timerTMP;
    TextMeshProUGUI _countdownTMP;

    public System.Action OnMenuOpen;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

            DisableAllUIExcept();

        _timerTMP = _ui_timer.GetComponent<TextMeshProUGUI>();
        _countdownTMP = _ui_countdown.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        LevelManager.Instance.OnOverview += EnableOverviewMessage;
        LevelManager.Instance.OnZoomIn += DisableAllUI;
        LevelManager.Instance.OnCountdownStart += EnableCountdown;
        LevelManager.Instance.OnGameplay += GameStarted;
        LevelManager.Instance.OnMenu += EnableGameMenu;
        LevelManager.Instance.OnFinished += GameEnded; 

        LevelManager.Instance.OnCountdownMessage += DefineCountdownMessage;
        InputDeviceMonitor.Instance.OnDeviceChanged += InputDeviceChanged;
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnOverview -= EnableOverviewMessage;
        LevelManager.Instance.OnZoomIn -= DisableAllUI;
        LevelManager.Instance.OnCountdownStart -= EnableCountdown;
        LevelManager.Instance.OnGameplay -= GameStarted;
        LevelManager.Instance.OnMenu -= EnableGameMenu;
        LevelManager.Instance.OnFinished += GameEnded;

        LevelManager.Instance.OnCountdownMessage -= DefineCountdownMessage;
        InputDeviceMonitor.Instance.OnDeviceChanged -= InputDeviceChanged;
    }

    private void Update()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(_timerManager.CurrentTime);

        string formattedTime = timeSpan.ToString(@"mm\:ss\.fff");

        _timerTMP.text = formattedTime;
    }

    private void InputDeviceChanged(DeviceType device)
    {
        DefineOverviewMessage(device);

        LevelState state = LevelManager.Instance.currentState;

        bool isTouch = device == DeviceType.Touch;
        bool isGameRunning = state == LevelState.Countdown || state == LevelState.Gameplay || state == LevelState.Finished;

        if (isTouch && isGameRunning) _ui_mobileControls.SetActive(true);
        else _ui_mobileControls.SetActive(false);
    }

    public void GameStarted()
    {
        DisableAllUIExcept(_ui_countdown, _ui_mobileControls, _ui_timer);
    }

    private void EnableCountdown()
    {
        DisableAllUIExcept();
        StopAllCoroutines();

        CheckForTouchOverlay();
        _ui_timer.SetActive(true);
        _ui_countdown.SetActive(true);
        StartCoroutine(CountdownRoutine());
    }

    private void EnableOverviewMessage()
    {
        DisableAllUIExcept();

        DefineOverviewMessage(InputDeviceMonitor.Instance.currentDevice);
        _ui_overviewMessage.SetActive(true);
    }

    private void EnableGameMenu()
    {
        DisableAllUIExcept(_ui_timer);

        _ui_gameMenu.SetActive(true);
        OnMenuOpen?.Invoke();
    }

    private void DefineOverviewMessage(DeviceType device)
    {
        TextMeshProUGUI overviewTMP = _ui_overviewMessage.GetComponent<TextMeshProUGUI>();

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
        TextMeshProUGUI countdownTMP = _ui_countdown.GetComponent<TextMeshProUGUI>();
        countdownTMP.text = message;
    }

    private void CheckForTouchOverlay()
    {
        if (InputDeviceMonitor.Instance.currentDevice == DeviceType.Touch)
        {
            if (!_ui_mobileControls.activeSelf) _ui_mobileControls.SetActive(true);
        }
        else _ui_mobileControls.SetActive(false);
    }

    private void DisableAllUIExcept(params GameObject[] uiElements)
    {
        if (!uiElements.Contains(_ui_overviewMessage)) _ui_overviewMessage.SetActive(false);
        if (!uiElements.Contains(_ui_mobileControls)) _ui_mobileControls.SetActive(false);
        if (!uiElements.Contains(_ui_countdown)) _ui_countdown.SetActive(false);
        if (!uiElements.Contains(_ui_gameMenu)) _ui_gameMenu.SetActive(false);
        if (!uiElements.Contains(_ui_timer)) _ui_timer.SetActive(false);
        if (!uiElements.Contains(_ui_overviewMessage)) _ui_endMenu.SetActive(false);
    }

    private void DisableAllUI()
    {
        DisableAllUIExcept();
    }

    private void GameEnded()
    {
        StartCoroutine(FinishRoutine());
    }

    private IEnumerator FinishRoutine()
    {
        // Give the player a few seconds to see their time
        CheckForTouchOverlay();
        WaitForSeconds wait = new(2f);
        yield return wait;

        // Change UI to end level menu
        DisableAllUI();

        _ui_endMenu.SetActive(true);
        OnMenuOpen?.Invoke();
    }

    private IEnumerator CountdownRoutine()
    {
        for (int i = 3; i > 0; i--) {
            _countdownTMP.text = i.ToString();
            yield return _waitForSeconds1;
        }
        _countdownTMP.text = "GO!";
        yield return _waitForSeconds1;
        _ui_countdown.SetActive(false);
    }
}
