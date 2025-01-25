using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private List<EnemyData> _enemyData;
    [SerializeField] private Vector2 _currentSpawnRateRange;
    private Vector2 _spawnOffset;
    private Vector2 _screenMiddle => new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

    private Timer _spawnTimer = new Timer();
    private ObjectPool<EnemyController> _enemyPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spawnOffset.x = Camera.main.orthographicSize * Camera.main.aspect;
        _spawnOffset.y = Camera.main.orthographicSize;

        _spawnTimer.OnTimerEnd.AddListener(SpawnEnemy);
        _spawnTimer.OnTimerEnd.AddListener(RestartSpawnTimer);

        _enemyPool = new ObjectPool<EnemyController>(_enemyPrefab.GetComponent<EnemyController>(), 200);

        RestartSpawnTimer();
    }

    // Update is called once per frame
    void Update()
    {
        _spawnTimer.Update();
    }

    void SpawnEnemy()
    {
        EnemyData data;
        if (Random.value > 0.5f)
        {
            data = _enemyData[0];
        }
        else
        {
            data = _enemyData[1];
        }
        Vector2 randomPosition = GetRandomPositionOutsideScreen();
        var enemy = _enemyPool.Get();
        enemy.transform.position = randomPosition;
        enemy.SetFlying(data.Flying);
        enemy.SetSprite(data.Sprite);
        enemy.SetDps(data.Damage);
        enemy.SetHealth(data.Health);
    }

    void RestartSpawnTimer()
    {
        _spawnTimer.StartTimer(Random.Range(_currentSpawnRateRange.x, _currentSpawnRateRange.y));
    }

    private Vector2 GetRandomPositionOutsideScreen()
    {
        float randomX = Random.Range(-_spawnOffset.x, _spawnOffset.x);
        float randomY = Random.Range(-_spawnOffset.y, _spawnOffset.y);

        // Ensure the enemy spawns outside the screen bounds
        if (Random.value > 0.5f)
        {
            randomX = randomX > 0 ? _spawnOffset.x + 1 : -_spawnOffset.x - 1;
        }
        else
        {
            randomY = randomY > 0 ? _spawnOffset.y + 1 : -_spawnOffset.y - 1;
        }

        return new Vector2(randomX, randomY) + _screenMiddle;
    }
}
