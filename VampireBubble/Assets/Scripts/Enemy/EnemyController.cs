using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    
    private float _health;
    private float _speed;
    private float _dps;
    private bool _flying;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;

    public bool Flying => _flying;
    public float Speed => _speed;
    public float Dps => _dps;
    [HideInInspector] public UnityEvent OnDeath = new UnityEvent();

    [SerializeField] private AudioClip[] _deathSounds = Array.Empty<AudioClip>();
    private AudioSource _audioSource;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _collider2D = GetComponent<Collider2D>();
        OnDeath.AddListener(PlayDeathSound);
    }

    public void Initialize(EnemyData data, Vector3 randomPosition)
    {
        _collider2D.enabled = true;
        transform.position = randomPosition;
        _speed = data.Speed;
        SetFlying(data.Flying);
        _spriteRenderer.sprite = data.Sprite;
        _dps = data.Damage;
        _health = data.Health;
    }

    private void SetFlying(bool flying)
    {
        _flying = flying;
    }

    public void Damage(float amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            StartCoroutine(Die());

        }
    }

    IEnumerator Die()
    {
        _collider2D.enabled = false;
        GameManager.Instance.EnemiesKilled++;
        OnDeath?.Invoke();
        PlayDeathSound();
        while (_audioSource.isPlaying)
        {
            yield return null;
        }
        EnemySpawner.Instance.OnDeath(this);
       
    }

    private void PlayDeathSound()
    {
        if (_deathSounds.Length > 0)
        {
            var randomIndex = Random.Range(0, _deathSounds.Length);
            _audioSource.resource = _deathSounds[randomIndex];
            _audioSource.Play();
        }
    }
}
