using UnityEngine;

public class PlayingState : GameStateBase
{
    public PlayingState(GameManager gameManager) : base(gameManager)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Entering Play");
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Exiting Play");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gameManager.ChangeState(new PausedState(_gameManager));
        }
        
    }
}
