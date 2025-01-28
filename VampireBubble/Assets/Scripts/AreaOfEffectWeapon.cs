using System;
using System.Collections;
using UnityEngine;

public class AreaOfEffectWeapon : WeaponBase
{
    [SerializeField] private AudioClip[] _attackSounds = Array.Empty<AudioClip>();
    private AudioSource _audioSource;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _attackEffect;
    private float _damage;
    private Vector2 _offsetFromPlayer;
    private Vector2 _damageBox;
    private float _duration;
    private float _debugDisplayTime;
    private bool _nearestEnemy;
    private bool _projectile;
    private float _remainingAttackTime = 0;

    public override void Initialize(WeaponData weaponData)
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = weaponData.Sprite;
        _spriteRenderer.enabled = true;
        _damage = weaponData.Damage;
        _damageBox.y = weaponData.BoxHeight;
        _damageBox.x = weaponData.BoxWidth;
        _duration = weaponData.Duration;
        _nearestEnemy = weaponData.NearestEnemy;
        _projectile = weaponData.Projectile;
        _attackCooldown = weaponData.AttackCooldown;
        _debugDisplayTime = weaponData.DebugDisplayTime;
        SetupAttackTimer();
    }

    private void Update()
    {
        if (!_nearestEnemy || _projectile)
        {
            Vector3 facingDir = GameManager.Instance.PlayerController.FacingDir();
            _attackEffect.transform.localScale = facingDir.x > 0 ? new Vector3(1, 1, -1) : new Vector3(1, 1, 1);
            
            transform.rotation = facingDir.x > 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
        }
    }

    protected override void Attack()
    {

        if (_animator != null)
        {
           _animator.Play(0);
        }
        else
        {
            TryHit();
        }


    }

    private IEnumerator AttackHelper(Vector3 areaBoxCenter)
    {
        while (_remainingAttackTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            DealDamage(areaBoxCenter);
            _remainingAttackTime -= 0.1f;
        }

    }

    public void TryHit()
    {
        Vector3 facingDir = GameManager.Instance.PlayerController.FacingDir();
        var playerPos = GameManager.Instance.PlayerController.transform.position;
        Vector3 areaBoxCenter = playerPos + facingDir * _damageBox.x * 0.5f;
        if (_nearestEnemy)
        {
            var enemy = EnemySpawner.Instance.GetNearestEnemy(GameManager.Instance.PlayerController.transform.position);
            areaBoxCenter = enemy.transform.position;
            transform.position = enemy.transform.position;
        }
       
        //StartCoroutine(ShowAttack());
        if (_duration != 0)
        {
            _remainingAttackTime = _duration;
            StartCoroutine(AttackHelper(areaBoxCenter));
        }
        else
        {
            DealDamage(areaBoxCenter);
        }
    }

    private void DealDamage(Vector3 areaBoxCenter)
    {
        StartCoroutine(ShowDamageEffect());
        var colliders = Physics2D.OverlapBoxAll(areaBoxCenter, _damageBox, 0);
        DrawDebugBox(areaBoxCenter);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Debug.Log($"Hit!!");
                var enemy = collider.GetComponent<EnemyController>();
                if (enemy)
                {
                    enemy.Damage(_damage);
                }
            }
        }
    }

    private IEnumerator ShowDamageEffect()
    {
        if (_attackEffect != null)
        {
            _attackEffect.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            _attackEffect.SetActive(false);
        }
    }

    private void DrawDebugBox(Vector3 center)
    {
        float halfWidth = _damageBox.x * 0.5f;
        float halfHeight = _damageBox.y * 0.5f;
        Vector3 topLeft = center + new Vector3(-halfWidth, halfHeight, 0);
        Vector3 topRight = center + new Vector3(halfWidth, halfHeight, 0);
        Vector3 bottomLeft = center + new Vector3(-halfWidth, -halfHeight, 0);
        Vector3 bottomRight = center + new Vector3(halfWidth, -halfHeight, 0);

        Debug.DrawLine(topLeft, topRight, Color.black, _debugDisplayTime);
        Debug.DrawLine(topRight, bottomRight, Color.black,  _debugDisplayTime);
        Debug.DrawLine(bottomRight, bottomLeft, Color.black,  _debugDisplayTime);
        Debug.DrawLine(bottomLeft, topLeft, Color.black,  _debugDisplayTime);
    }
}
