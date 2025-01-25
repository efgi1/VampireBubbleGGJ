using UnityEngine;

public class GameStateBase
{
    protected GameManager _gameManager;

    public GameStateBase(GameManager gameManager)
    {
        this._gameManager = gameManager;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }
}
