using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _HUD;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    public void SetMainMenuVisible(bool visible)
    {
        _mainMenu.SetActive(visible);
    }

    public void SetGameOverMenuVisible(bool visible)
    {
        _gameOverMenu.SetActive(visible);
    }

    public void SetPauseMenuVisible(bool visible)
    {
        _pauseMenu.SetActive(visible);
    }

    public void SetHUDVisible(bool visible)
    {
        _HUD.SetActive(visible);
    }
}
