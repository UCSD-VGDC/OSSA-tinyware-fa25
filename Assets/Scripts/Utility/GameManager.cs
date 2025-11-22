using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int level;
    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            if (enemyPrefabs.Count == 0) return;
            if (enemyPrefabs[0].Key == level)
            {
                enemyPool.Add(enemyPrefabs[0].Value);
                enemyPrefabs.RemoveAt(0);
            }
        }
    }

    public int ExpNeeded => GetExpForLevel(Level);
    public float EnemySpeedMultiplier => GetEnemySpeedMultiplier(Level);
    public (float,float) EnemySpawnIntervalRange => GetEnemySpawnIntervalRange(Level);
    [SerializeField] private List<MyKeyValuePair<int, GameObject>> enemyPrefabs;
    [SerializeField] private List<GameObject> enemySpawnPoints;
    [SerializeField] private Weapon startingWeapon;

    private List<GameObject> enemyPool = new();
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Level = 1;
        Player.Instance.OnStart(startingWeapon);
        enemyPool = new List<GameObject>{ enemyPrefabs[0].Value };
        enemyPrefabs.RemoveAt(0);

        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            float waitTime = Random.Range(EnemySpawnIntervalRange.Item1, EnemySpawnIntervalRange.Item2);
            yield return new WaitForSeconds(waitTime);

            int randomIndex = Random.Range(0, enemyPool.Count);
            GameObject enemyPrefab = enemyPool[randomIndex];
            Vector3 spawnPosition = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Count)].transform.position;
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private int GetExpForLevel(int level) { return level * 5; }
    private float GetEnemySpeedMultiplier(int level) { return 2 + level * 0.1f; }
    private (float, float) GetEnemySpawnIntervalRange(int level)
    {
        float min = Mathf.Max(0.5f, 3.0f - level * 0.1f);
        float max = Mathf.Max(1.0f, 5.0f - level * 0.2f);
        return (min, max);
    }
}
