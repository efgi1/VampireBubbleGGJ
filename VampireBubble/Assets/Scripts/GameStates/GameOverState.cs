using UnityEngine;

public class GameOverState: GameStateBase
{
    public GameOverState(GameManager gameManager) : base(gameManager)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Time.timeScale = 0;
        UIManager.Instance.SetGameOverMenuVisible(true);
        UIManager.Instance.SetHUDVisible(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        _gameManager.TimeElapsedSinceStart = 0;
        Time.timeScale = 1;
        UIManager.Instance.SetGameOverMenuVisible(false);
        UIManager.Instance.SetHUDVisible(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _gameManager.ChangeState(new MenuState(_gameManager));
        }
    }
}
