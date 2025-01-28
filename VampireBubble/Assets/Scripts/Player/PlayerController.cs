using System;
using System.Collections;
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
    public Animator Animator;
    // Weapons
    [SerializeField] private WeaponData[] _weaponData;
    [SerializeField] private List<WeaponBase> _weapons = new List<WeaponBase>();
    public GameObject ClubWeaponPrefab;
    public GameObject TackWeaponPrefab;

    [SerializeField] private GameObject _shieldObject;
    private float shieldTime = 0;

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
     public float _experienceToNextLevel = 10f;
     
     public UnityEvent<float> OnExperienceNeededChange = new();
     public float ExperienceToNextLevel
     {
         get => _experienceToNextLevel;
         set { 
             OnExperienceNeededChange?.Invoke(value); 
             _experienceToNextLevel = value;
         }
     }
     public float _experience = 0;

     
     public UnityEvent<float> OnExperienceChange = new();
     public float Experience
     {
         get => _experience;
         set { 
             OnExperienceChange?.Invoke(value); 
             _experience = value;
         }
     }
     private int _level = 1;
     public int Level
     {
         get => _level;
         set { 
             OnLevelUp?.Invoke(value); 
             _level = value;
         }
     }

     public UnityEvent<int> OnLevelUp = new();

    // Audio
    [SerializeField] private AudioClip[] _levelUpSounds = Array.Empty<AudioClip>();
    [SerializeField] private AudioClip[] _damageSounds = Array.Empty<AudioClip>();
     [SerializeField] private AudioClip[] _deathSounds = Array.Empty<AudioClip>();
     private AudioSource _audioSource;


     //Death
     public UnityAction DeathEvent;

    void Awake()
    {
        Animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _collider = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();

          _currentHealth = _heroDataSO.MaxHealth;
          _damageDelayTimer = _heroDataSO.DamageDelayTime;
    }

    void Start()
    {
        GameObject newWeaponObject = Instantiate(ClubWeaponPrefab, transform);
        WeaponBase newWeapon = newWeaponObject.GetComponent<AreaOfEffectWeapon>();
        newWeapon.Initialize(_weaponData[0]);
        _weapons.Add(newWeapon);

        GameManager.Instance.OnKillCountChange.AddListener(GainExperience);
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

        shieldTime -= Time.deltaTime;
        if (shieldTime <= 0)
        {
            _shieldObject.SetActive(false);
        }
        _damageDelayTimer -= Time.deltaTime;

        UpdateWeapons();
    }

     private void TakeDamage(float damageTaken)
     {
         if (shieldTime > 0) return;
          // Wait for timer to take damage
          if(_damageDelayTimer > 0) { return; }
          _damageDelayTimer = _heroDataSO.DamageDelayTime;

          _currentHealth -= damageTaken;
          HealthChangeEvent?.Invoke(_currentHealth);

          if (_currentHealth < 0)
          {
               Debug.Log("Health reached zero: Game Over");
               DeathEvent?.Invoke();
               StartCoroutine(PlayDeathSequence());

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

     private IEnumerator PlayDeathSequence()
     {
         var randomIndex = UnityEngine.Random.Range(0, _deathSounds.Length);
         _audioSource.PlayOneShot(_deathSounds[randomIndex], 1f);
         GameManager.Instance.PlayerController.Animator.Play("Player_Death_Animation");
         

         GameManager.Instance.ChangeState(new GameOverState(GameManager.Instance));

         // Wait for the animation and sound to finish
         while (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 || _audioSource.isPlaying)
         {
             Animator.Update(Time.unscaledDeltaTime);
             yield return null;
         }
     }

     public void ApplyPickup(PickupData data)
     {
        switch(data.Type)
        {
            case PickupType.Health:
                _currentHealth += data.Value;
                HealthChangeEvent?.Invoke(_currentHealth);
                break;
            //case PickupType.Experience:
            //    _experience += data.Value;
            //    break;
            case PickupType.NailUpgrade:
                if (_weapons.Count < 2)
                {
                    GameObject newWeaponObject = Instantiate(TackWeaponPrefab);
                    WeaponBase newWeapon = newWeaponObject.GetComponent<AreaOfEffectWeapon>();
                    newWeapon.Initialize(_weaponData[1]);
                    _weapons.Add(newWeapon);
                }
                break;
            case PickupType.RoseUpgrade:
                Experience += 10;
                break;
            case PickupType.ClubUpgrade:
                // Add weapon to player
                break;
            case PickupType.Bomb:
                ApplyBomb();
                break;
            case PickupType.Shield:
                shieldTime = data.Value;
                _shieldObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

     private void ApplyBomb()
     {
        GameManager.Instance.EnemiesKilled += EnemySpawner.Instance.GetEnemyCount();
        EnemySpawner.Instance.ClearEnemies();
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
        Experience = 0;
        Level = 1;
        transform.position = Vector3.zero;
        ExperienceToNextLevel = 10;

    }

    public void GainExperience(float killCount)
    {
        if (killCount == 0)
        {
            Experience = 0;
            return;
        }
        Experience++;
        if (Experience >= ExperienceToNextLevel)
        {
            Experience -= ExperienceToNextLevel;
            ExperienceToNextLevel *= 1.5f;
            Level++;
            var randomIndex = UnityEngine.Random.Range(0, _levelUpSounds.Length);
            _audioSource.PlayOneShot(_levelUpSounds[randomIndex]);

        }
    }

    internal Vector3 FacingDir()
    {
        return _spriteRenderer.flipX ? -transform.right : transform.right;
    }
}
