using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _HUD;
    [SerializeField] private TMP_Text _scoreText;

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

    private void Start()
    {
        GameManager.Instance.OnKillCountChange.AddListener(UpdateKillCountText);
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

    private void UpdateKillCountText(float killCount)
    {
        _scoreText.text = $"{killCount}";
    }
}
