using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private List<EnemyData> _enemyData;
    [SerializeField] private Vector2 _currentSpawnRateRange;
    [SerializeField] private AnimationCurve _randomSizeCurve;
    private Vector2 _spawnOffset;
    private Vector2 _screenMiddle => new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

    private Timer _spawnTimer = new Timer();
    private ObjectPool<EnemyController> _enemyPool;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    private void SpawnEnemy()
    {
        EnemyData data;
        if (Random.value > 0.5f)
        {
            data = _enemyData[0];
        }
        else
        {
            data = _enemyData[0];
        }
        Vector2 randomPosition = GetRandomPositionOutsideScreen();
        var enemy = _enemyPool.Get();
        enemy.Initialize(data, randomPosition);
        float randomSize = GetRandomValueFromCurve(_randomSizeCurve);
        enemy.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        enemy.GetComponent<SpriteRenderer>().color = GetRandomLightColor();
    }

    private float GetRandomValueFromCurve(AnimationCurve curve)
    {
        if (curve == null || curve.length == 0)
        {
            Debug.LogWarning("Curve is not defined or has no keys.");
            return 0f;
        }

        // Get the time range of the curve
        float startTime = curve.keys[0].time;
        float endTime = curve.keys[curve.length - 1].time;

        // Generate a random time within the range
        float randomTime = Random.Range(startTime, endTime);

        // Evaluate the curve at the random time
        return curve.Evaluate(randomTime);
    }

    private Color GetRandomLightColor()
    {
        float r = Random.Range(0.5f, 1f);
        float g = Random.Range(0.5f, 1f);
        float b = Random.Range(0.5f, 1f);
        return new Color(r, g, b);
    }

    private void RestartSpawnTimer()
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

    public void OnDeath(EnemyController controller)
    {
        _enemyPool.ReturnToPool(controller);
    }

    public void ClearEnemies()
    {
        _enemyPool.ReturnAllToPool();
    }

    public int GetEnemyCount()
    {
        return FindObjectsByType<EnemyController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;
    }

    public EnemyController GetNearestEnemy(Vector3 position)
    {
        EnemyController[] objects =
            FindObjectsByType<EnemyController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        EnemyController nearestObject = null;
        float minDistance = float.MaxValue;

        foreach (EnemyController obj in objects)
        {
            float distance = Vector3.Distance(position, obj.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestObject = obj;
            }
        }

        return nearestObject;
    }
}
