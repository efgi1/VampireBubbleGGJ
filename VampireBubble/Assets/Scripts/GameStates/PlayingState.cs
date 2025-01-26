using UnityEngine;

public class PlayingState : GameStateBase
{
    private bool _resetState;
    public PlayingState(GameManager gameManager, bool resetState = true) : base(gameManager)
    {
        _resetState = resetState;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Entering Play");
        UIManager.Instance.SetHUDVisible(true);
        if (_resetState)
        {
            GameManager.Instance.EnemiesKilled = 0;
            GameManager.Instance.PlayerController.ResetForNewGame();
            EnemySpawner.Instance.ClearEnemies();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Exiting Play");
        UIManager.Instance.SetHUDVisible(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gameManager.ChangeState(new PausedState(_gameManager));
        }

        _gameManager.TimeElapsedSinceStart += Time.deltaTime;

    }
}
