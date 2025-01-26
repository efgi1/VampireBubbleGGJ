using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _HUD;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Slider _experienceSlider;

    private void OnDestroy()
    {
    }

    private void OnExperienceChanged(float newExperience)
    {
        _experienceSlider.value = newExperience;
    }

    private void OnExperienceToNextLevelChange(float experience)
    {
        _experienceSlider.maxValue = experience;
    }

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
        GameManager.Instance.PlayerController.OnLevelUp.AddListener(UpdateLevelText);
        _experienceSlider.maxValue = GameManager.Instance.PlayerController.ExperienceToNextLevel;
        _experienceSlider.value = 0;

        GameManager.Instance.PlayerController.OnExperienceChange.AddListener(OnExperienceChanged);
        GameManager.Instance.PlayerController.OnExperienceNeededChange.AddListener(OnExperienceToNextLevelChange);
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
    private void UpdateLevelText(int level)
    {
        _levelText.text = $"{level}";
    }
}
