using UnityEngine;
using UnityEngine.Events;

public class Timer
{
    public UnityEvent OnTimerEnd = new UnityEvent();
    [SerializeField] private float _timeRemaining;
    private bool _timerIsRunning;

    public void Update()
    {
        if (_timerIsRunning)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
            }
            else
            {
                _timeRemaining = 0;
                _timerIsRunning = false;
                OnTimerEnd?.Invoke();
            }
        }
    }

    public void StartTimer(float duration)
    {
        _timeRemaining = duration;
        _timerIsRunning = true;
    }

    public void StopTimer()
    {
        _timerIsRunning = false;
    }

    public void ResetTimer(float duration)
    {
        _timeRemaining = duration;
        _timerIsRunning = false;
    }

    public float GetTimeRemaining()
    {
        return _timeRemaining;
    }

    public bool IsTimerRunning()
    {
        return _timerIsRunning;
    }
}
