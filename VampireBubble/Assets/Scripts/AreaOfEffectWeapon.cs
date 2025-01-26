using System;
using System.Collections;
using UnityEngine;

public class AreaOfEffectWeapon : WeaponBase
{
    [SerializeField] private AudioClip[] _attackSounds = Array.Empty<AudioClip>();
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private float _damage;
    private Vector2 _offsetFromPlayer;
    private Vector2 _damageBox;
    private float _duration;
    private float _debugDisplayTime;
    private bool _nearestEnemy;
    private bool _projectile;

    public override void Initialize(WeaponData weaponData)
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = weaponData.Sprite;
        _spriteRenderer.enabled = false;
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

    protected override void Attack()
    {
        Vector3 facingDir = GameManager.Instance.PlayerController.FacingDir();
        transform.localPosition = new Vector3(facingDir.x, 0, 0);
        Vector3 areaBoxCenter = transform.position + facingDir * _damageBox.x * 0.5f - facingDir;
        var colliders = Physics2D.OverlapBoxAll(areaBoxCenter, _damageBox, 0);
        DrawDebugBox(areaBoxCenter);
        StartCoroutine(ShowAttack());
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

    private IEnumerator ShowAttack()
    {
        _spriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.25f);
        _spriteRenderer.enabled = false;

    }
    private void DrawDebugBox(Vector3 center)
    {
        float halfWidth = _damageBox.x * 0.5f;
        float halfHeight = _damageBox.y * 0.5f;
        Vector3 topLeft = center + new Vector3(-halfWidth, halfHeight, 0);
        Vector3 topRight = center + new Vector3(halfWidth, halfHeight, 0);
        Vector3 bottomLeft = center + new Vector3(-halfWidth, -halfHeight, 0);
        Vector3 bottomRight = center + new Vector3(halfWidth, -halfHeight, 0);

        Debug.DrawLine(topLeft, topRight, Color.yellow, _debugDisplayTime);
        Debug.DrawLine(topRight, bottomRight, Color.yellow,  _debugDisplayTime);
        Debug.DrawLine(bottomRight, bottomLeft, Color.yellow,  _debugDisplayTime);
        Debug.DrawLine(bottomLeft, topLeft, Color.yellow,  _debugDisplayTime);
    }
}
