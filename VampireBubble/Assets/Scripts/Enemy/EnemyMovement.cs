using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _avoidanceRadius = 1.0f;
    private EnemyController _controller;
    private PlayerController[] _players; // TODO player access anywhere, one lookup
    private Collider2D _collider;


    void Start()
    {
        _players = FindObjectsByType<PlayerController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        _collider = GetComponent<Collider2D>();
        _controller = GetComponent<EnemyController>();
        if (_controller.Flying)
        {
            gameObject.layer = LayerMask.NameToLayer("FlyingEnemy");
        }

        _avoidanceRadius *= transform.localScale.x;
    }

    void Update()
    {
        Vector3 playerDir = (_players[0].transform.position - transform.position).normalized;
        Vector3 avoidanceDir = GetSeparationDirection();
        Vector3 moveDir = (playerDir + avoidanceDir).normalized;

        transform.position += moveDir * _controller.Speed * Time.deltaTime;
    }

    

    private Vector3 GetSeparationDirection()
    {
        Vector3 separationDir = Vector3.zero;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _avoidanceRadius);
        foreach (Collider2D collider in colliders)
        {
            bool shouldAvoid = collider.CompareTag("Enemy") 
                               || collider.CompareTag("TallObstacle")
                               || (!_controller.Flying && collider.CompareTag("ShortObstacle"));
                
            if (collider != _collider && shouldAvoid)
            {
                Vector3 diff = transform.position - collider.transform.position;
                if (diff.magnitude != 0)
                {
                    separationDir += diff.normalized / diff.magnitude;
                }
            }
        }
        return separationDir;
    }
}
