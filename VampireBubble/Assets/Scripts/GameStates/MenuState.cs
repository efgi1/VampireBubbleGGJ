using UnityEngine;

public class MenuState : GameStateBase
{
    public MenuState(GameManager gameManager) : base(gameManager)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Time.timeScale = 0;
        UIManager.Instance.SetMainMenuVisible(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        Time.timeScale = 1;
        UIManager.Instance.SetMainMenuVisible(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _gameManager.ChangeState(new PlayingState(_gameManager));
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
