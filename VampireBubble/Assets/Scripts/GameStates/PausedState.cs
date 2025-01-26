using UnityEngine;

public class PausedState : GameStateBase
{

    public PausedState(GameManager gameManager) : base(gameManager)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Time.timeScale = 0;
        Debug.Log("Entering Pause");
        UIManager.Instance.SetHUDVisible(true);
        UIManager.Instance.SetPauseMenuVisible(true);

    }

    public override void OnExit()
    {
        base.OnExit();
        Time.timeScale = 1;
        Debug.Log("Exiting Pause");
        UIManager.Instance.SetPauseMenuVisible(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gameManager.ChangeState(new PlayingState(_gameManager, false));
        }
    }
}
