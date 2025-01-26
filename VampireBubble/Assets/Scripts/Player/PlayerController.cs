using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Weapons
    [SerializeField] private WeaponData[] _weaponData;
    private List<WeaponBase> _weapons = new List<WeaponBase>();
    public GameObject WeaponPrefab;

    // Hit Color
    [SerializeField] private float _colorChangeSpeed = 0.05f;
    [SerializeField] private Color _damageColor;
    private Color _originalColor;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    

     // Health
     [SerializeField] public HeroData _heroDataSO;
     [SerializeField] private float _currentHealth = 10;
     public UnityAction<float> HealthChangeEvent;
     private float _damageDelayTimer;
     private List<float> _collidingMonsterDamages = new List<float>();
     private float _maxCurrentDamage = 0;

     // Experience
     private float _experience = 0;
     private int _level = 1;

     // Audio
     [SerializeField] private AudioClip[] _damageSounds = Array.Empty<AudioClip>();
     [SerializeField] private AudioClip[] _deathSounds = Array.Empty<AudioClip>();
     private AudioSource _audioSource;


     //Death
     public UnityAction DeathEvent;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _collider = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();

          _currentHealth = _heroDataSO.MaxHealth;
          _damageDelayTimer = _heroDataSO.DamageDelayTime;
    }

    void Start()
    {
        foreach(WeaponData weapon in _weaponData)
        {
            GameObject newWeaponObject = Instantiate(WeaponPrefab, transform);
            WeaponBase newWeapon = newWeaponObject.AddComponent<AreaOfEffectWeapon>();
            newWeapon.Initialize(weapon);
            _weapons.Add(newWeapon);
        }
    }

    void Update()
    {
        if (_maxCurrentDamage > 0)
        {
            _spriteRenderer.color = Color.Lerp(_spriteRenderer.color , _damageColor, _colorChangeSpeed);
               // Can adjust how much damage is taken here if we want to.
               TakeDamage(_maxCurrentDamage);
        }
        else
        {
            _spriteRenderer.color = Color.Lerp(_spriteRenderer.color , _originalColor, _colorChangeSpeed);
        }

        _damageDelayTimer -= Time.deltaTime;

        UpdateWeapons();
    }

     private void TakeDamage(float damageTaken)
     {
          // Wait for timer to take damage
          if(_damageDelayTimer > 0) { return; }
          _damageDelayTimer = _heroDataSO.DamageDelayTime;

          _currentHealth -= damageTaken;
          HealthChangeEvent?.Invoke(_currentHealth);

          if (_currentHealth < 0)
          {
               Debug.Log("Health reached zero: Game Over");
               DeathEvent?.Invoke();
               var randomIndex = UnityEngine.Random.Range(0, _deathSounds.Length);
               _audioSource.PlayOneShot(_deathSounds[randomIndex], 1f);
               GameManager.Instance.ChangeState(new GameOverState(GameManager.Instance));

               return;
          }

          if (_damageSounds.Length > 0)
          {
              var randomIndex = UnityEngine.Random.Range(0, _damageSounds.Length);
              if (!_audioSource.isPlaying)
              {
                  _audioSource.PlayOneShot(_damageSounds[randomIndex], 1f);
              }
          }
     }

     public void OnWeaponPickup()
     {

     }

     private void UpdateWeapons()
    {
        foreach (WeaponBase weapon in _weapons)
        {
            weapon.UpdateAttackTimer();
        }
    }


    public void HandleEnemyCollisionEnter(Collider2D other)
     {
         _collidingMonsterDamages.Add(other.GetComponent<EnemyController>().Dps);
         _maxCurrentDamage = _collidingMonsterDamages.Count > 0 ? _collidingMonsterDamages.Max() : 0;
        //Debug.Log($"Colliding with {++_enemyCollisionCount} enemies");
    }

    public void HandleEnemyCollisionExit(Collider2D other)
    {
        _collidingMonsterDamages.Remove(other.GetComponent<EnemyController>().Dps);
        _maxCurrentDamage = _collidingMonsterDamages.Count > 0 ? _collidingMonsterDamages.Max() : 0;
        //Debug.Log($"Colliding with {--_enemyCollisionCount} enemies");
    }

    public void ResetForNewGame()
    {
        _currentHealth = _heroDataSO.MaxHealth;
        HealthChangeEvent?.Invoke(_currentHealth);
        _experience = 0;
        _level = 1;
        transform.position = Vector3.zero;
    }
    
}
