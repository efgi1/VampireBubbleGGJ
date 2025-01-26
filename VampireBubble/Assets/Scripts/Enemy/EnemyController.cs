using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;

public class EnemyController : MonoBehaviour
{
    private float _health;
    private float _speed;
    private float _dps;
    private bool _flying;
    private SpriteRenderer _spriteRenderer;

    public bool Flying => _flying;
    public float Speed => _speed;
    public float Dps => _dps;
    public UnityEvent OnDeath = new UnityEvent();

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(EnemyData data, Vector3 randomPosition)
    {
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
        // below is temp
        if (flying)
        {
            _spriteRenderer.flipY = true;
            _spriteRenderer.color = Color.red;
        }
        else
        {
            _spriteRenderer.flipY = false;
            _spriteRenderer.color = Color.green;
        }
    }

    public void Damage(float amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            EnemySpawner.Instance.OnDeath(this);
            OnDeath?.Invoke();
        }
    }
}
