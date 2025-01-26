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
    }

    public override void OnExit()
    {
        base.OnExit();
        Time.timeScale = 1;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
