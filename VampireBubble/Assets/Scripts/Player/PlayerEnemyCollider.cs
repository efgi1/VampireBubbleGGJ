using UnityEngine;

public class PlayerEnemyCollider : MonoBehaviour
{
    private bool bCollidingWithEnemy = false;
    private PlayerController _playerController;

    void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            _playerController.HandleEnemyCollisionEnter(other);
        }
        if (other.CompareTag("Pickup"))
        {
            var pickup = other.GetComponent<Pickup>();
            _playerController.ApplyPickup(pickup.Data);
            pickup.OnPickup();
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
