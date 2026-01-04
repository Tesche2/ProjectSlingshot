using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class JoystickHandler : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    [SerializeField] private RectTransform _stickThumb;
    [SerializeField] private RectTransform _stickBackground;
    [SerializeField] private float _maxStickRange = 25;
    [SerializeField]
    [Range(0f, 1f)] private float _deadZone = 0.1f;

    [InputControl(layout = "Vector2")]
    [SerializeField] private string _controlPath;

    private Vector2 _inputVector;
    private RectTransform _stickRegion;

    private void Awake()
    {
        _stickRegion = GetComponent<RectTransform>();
    }

    protected override string controlPathInternal
    { 
        get => _controlPath;
        set => _controlPath = value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CalculateInput(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        CalculateInput(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetStick();
    }

    private void CalculateInput(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _stickRegion,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        localPoint = localPoint - _stickBackground.anchoredPosition;

        if( localPoint.magnitude / _maxStickRange < _deadZone)
        {
            ResetStick();
            return;
        }

        Vector2 clampedPosition = localPoint;
        if (clampedPosition.magnitude > _maxStickRange)
        {
            clampedPosition = clampedPosition.normalized * _maxStickRange;
        }

        _stickThumb.anchoredPosition = clampedPosition;

        _inputVector = clampedPosition / _maxStickRange;

        SendValueToControl(_inputVector);
    }

    private void ResetStick()
    {
        _inputVector = Vector2.zero;
        _stickThumb.anchoredPosition = Vector2.zero;

        SendValueToControl(Vector2.zero);
    }
}
