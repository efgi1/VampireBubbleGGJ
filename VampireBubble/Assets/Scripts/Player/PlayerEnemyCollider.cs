using UnityEngine;

public class PlayerEnemyCollider : MonoBehaviour
{
    private bool bCollidingWithEnemy = false;
    private PlayerController _playerController;

    void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            _playerController.HandleEnemyCollisionEnter(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            _playerController.HandleEnemyCollisionExit(other);
        }
    }
}
