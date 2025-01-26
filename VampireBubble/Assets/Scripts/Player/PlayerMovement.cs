using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField]  InputActionAsset _inputActions;
    private Vector2 _movement;
    private InputAction _moveAction;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    void Awake()
    {
        _moveAction = _inputActions.FindActionMap("Player").FindAction("Move");
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        _moveAction.Enable();
        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;
    }

    void OnDisable()
    {
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMove;
        _moveAction.Disable();
    }


    void OnMove(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        // Move the player using Rigidbody2D velocity to ensure proper collision handling
        _rb.linearVelocity = _movement * _moveSpeed;
        // Flip the player sprite based on movement direction
        if (_movement.x != 0)
            _spriteRenderer.flipX = _movement.x < 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        _movement = Vector2.zero;
        _rb.linearVelocity = Vector2.zero;
        Debug.Log("Collided with " + collision.gameObject.name);
    }
}
