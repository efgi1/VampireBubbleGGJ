using System;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _colorChangeSpeed = 0.05f;
    private Color _originalColor;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private int _enemyCollisionCount = 0;

     // Health
     [SerializeField] public HeroData _heroDataSO;
    [SerializeField]
    private int _currentHealth = 10;
     public UnityAction<int> HealthChangeEvent;
     private float _damageDelayTimer;

     //Death
     public UnityAction DeathEvent;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _collider = GetComponent<Collider2D>();

          _currentHealth = _heroDataSO.MaxHealth;
          _damageDelayTimer = _heroDataSO.DamageDelayTime;
    }

    void Update()
    {
        if (_enemyCollisionCount > 0)
        {
            _spriteRenderer.color = Color.Lerp(_spriteRenderer.color , Color.red, _colorChangeSpeed);
               // Can adjust how much damage is taken here if we want to.
               TakeDamage(1);
        }
        else
        {
            _spriteRenderer.color = Color.Lerp(_spriteRenderer.color , _originalColor, _colorChangeSpeed);
          }

        _damageDelayTimer -= Time.deltaTime;
     }

     private void TakeDamage(int damageTaken)
     {
          // Wait for timer to take damage
          if(_damageDelayTimer > 0) { return; }
          _damageDelayTimer = _heroDataSO.DamageDelayTime;

          _currentHealth -= damageTaken;
          HealthChangeEvent?.Invoke(_currentHealth);

          if (_currentHealth < 0)
          {
               // TODO: impement game over
               Debug.Log("Health reached zero: Game Over");
               _currentHealth = _heroDataSO.MaxHealth;
               HealthChangeEvent?.Invoke(_currentHealth);

               // TODO: Implement death
               DeathEvent?.Invoke();
          }
     }


     public void HandleEnemyCollisionEnter(Collider2D other)
    {
        Debug.Log($"Colliding with {++_enemyCollisionCount} enemies");
    }

    public void HandleEnemyCollisionExit(Collider2D other)
    {
        Debug.Log($"Colliding with {--_enemyCollisionCount} enemies");
    }
    
}
