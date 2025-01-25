using System;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _colorChangeSpeed = 0.05f;
    private Color _originalColor;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private int _enemyCollisionCount = 0;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (_enemyCollisionCount > 0)
        {
            _spriteRenderer.color = Color.Lerp(_spriteRenderer.color , Color.red, _colorChangeSpeed);
        }
        else
        {
            _spriteRenderer.color = Color.Lerp(_spriteRenderer.color , _originalColor, _colorChangeSpeed);
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
