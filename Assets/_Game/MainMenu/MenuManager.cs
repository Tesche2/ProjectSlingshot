using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        GlobalInputManager.Instance.SetInputState_Menu();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in canvas.transform)
        { 
            child.gameObject.SetActive(false);
        }

        mainPanel.SetActive(true);
    }

    public void OnExitPress()
    {
        Application.Quit();
    }
}
