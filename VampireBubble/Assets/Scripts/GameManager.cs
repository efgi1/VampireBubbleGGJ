using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameStateBase _currentState;
    public GameStateBase CurrentState => _currentState;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ChangeState(new PlayingState(this));
    }

    // Update is called once per frame
    void Update()
    {
        _currentState?.OnUpdate();
    }

    public void ChangeState(GameStateBase newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter();
    }
}
