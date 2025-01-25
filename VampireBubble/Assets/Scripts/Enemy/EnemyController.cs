using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float _health;
    private float _speed;
    private float _dps;
    private bool _flying;
    private SpriteRenderer _spriteRenderer;

    public bool Flying => _flying;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
    }

    void Update()
    {
        
    }

    public void SetHealth(float health)
    {
        _health = health;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetDps(float dps)
    {
        _dps = dps;
    }

    public void SetFlying(bool flying)
    {
        _flying = flying;
        if (flying)
        {
            _spriteRenderer.flipY = true;
            _spriteRenderer.color = Color.red;
        }
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
