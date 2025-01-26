using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected Timer _attackTimer = new Timer();
    protected float _attackCooldown;

    public abstract void Initialize(WeaponData Data);

    public void UpdateAttackTimer()
    {
        _attackTimer.Update();
    }

    protected virtual void SetupAttackTimer()
    {
        _attackTimer.OnTimerEnd.AddListener(Attack);
        _attackTimer.OnTimerEnd.AddListener(StartAttackCooldown);
        StartAttackCooldown();
    }

    protected abstract void Attack();

    private void StartAttackCooldown()
    {
        _attackTimer.StartTimer(_attackCooldown);
    }

    private void StopAttack()
    {
        _attackTimer.StopTimer();
    }
}
