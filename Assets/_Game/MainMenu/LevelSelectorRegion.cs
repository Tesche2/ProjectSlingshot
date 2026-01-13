using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(GridLayoutGroup))]
public class LevelSelectorRegion : MonoBehaviour
{
    [SerializeField] private int levelsQuantity;
    [SerializeField] private int spacerMinWidth = 8;
    [SerializeField] private GameObject buttonPrefab;

    private int _nCols = 0;
    private int _nRows = 0;
    private RectTransform _rectTransform;
    private GridLayoutGroup _gridLayoutGroup;

    private void OnEnable()
    {
        if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
        if (_gridLayoutGroup == null) _gridLayoutGroup = GetComponent<GridLayoutGroup>();

        _gridLayoutGroup.cellSize = Vector2.zero;
        PlaceButtons();
    }

    private void OnRectTransformDimensionsChange()
    {
        if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
        if (_gridLayoutGroup == null) _gridLayoutGroup = GetComponent<GridLayoutGroup>();

        _gridLayoutGroup.cellSize = Vector2.zero;
        CancelInvoke(nameof(PlaceButtons));
        Invoke(nameof(PlaceButtons), 0.1f);
    }

    private void PlaceButtons()
    {
        FindBestGrid();
        Debug.Log("Rows: " + _nRows + " Cols: " + _nCols);
        CalculateGridDimensions();
        if (transform.childCount == 0)
        {
            InstantiateButtons();
        }
    }

    private void FindBestGrid()
    {
        if (_rectTransform.rect.height <= 0) return;

        float rectRatio = _rectTransform.rect.width / _rectTransform.rect.height;
        float minDiff = float.MaxValue;

        _nCols = Mathf.CeilToInt(Mathf.Sqrt(levelsQuantity));
        _nRows = Mathf.CeilToInt((float)levelsQuantity / _nCols);

        for (int i = 1; i * i <= levelsQuantity; i++)
        {
            if (levelsQuantity % i == 0)
            {
                int rows = i;
                int cols = levelsQuantity / i;

                // Check ratio
                float gridRatio = (float)cols / (float)rows;
                float ratioDiff = Mathf.Abs(gridRatio - rectRatio);

                if (ratioDiff < minDiff)
                {
                    minDiff = ratioDiff;
                    _nCols = cols;
                    _nRows = rows;
                }
            }
        }
    }

    private void CalculateGridDimensions()
    {
        float effectiveWidth = _rectTransform.rect.width - _gridLayoutGroup.padding.left - _gridLayoutGroup.padding.right;
        float effectiveHeight = _rectTransform.rect.height - _gridLayoutGroup.padding.top - _gridLayoutGroup.padding.bottom;

        float buttonSize = Mathf.Min(
            (effectiveWidth - (spacerMinWidth * (_nCols - 1))) / _nCols,
            (effectiveHeight - (spacerMinWidth * (_nRows - 1))) / _nRows
        );

        _gridLayoutGroup.cellSize = new Vector2(buttonSize, buttonSize);

        float horizontalSpacing = (effectiveWidth - (buttonSize * _nCols)) / (_nCols - 1);
        float verticalSpacing = (effectiveHeight - (buttonSize * _nRows)) / (_nRows - 1);

        _gridLayoutGroup.spacing = new Vector2(horizontalSpacing, verticalSpacing);
    }

    private void InstantiateButtons()
    {
        for (int i = 0; i < levelsQuantity; i++)
        {
            GameObject button = Instantiate(buttonPrefab, transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = (i+1).ToString();

            Button btnComponent = button.GetComponent<Button>();
            btnComponent.onClick.AddListener(() =>
            {
                SceneLoader.Instance.LoadScene("Level_1");
            });
        }
    }
}
