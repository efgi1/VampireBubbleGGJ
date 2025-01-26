using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameStateBase CurrentState { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public float TimeElapsedSinceStart { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayerController = FindAnyObjectByType<PlayerController>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ChangeState(new MenuState(this));
    }

    // Update is called once per frame
    void Update()
    {
        CurrentState?.OnUpdate();
    }

    public void ChangeState(GameStateBase newState)
    {
        CurrentState?.OnExit();
        CurrentState = newState;
        CurrentState.OnEnter();
    }
}
