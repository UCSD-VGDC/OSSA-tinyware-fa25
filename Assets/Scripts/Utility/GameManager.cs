using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Combat,
        Upgrade
    }

    public static GameManager Instance;
    public GameState CurrentState { get; private set; } = GameState.Menu;

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
    private List<GameObject> enemyPool = new();
    [SerializeField] private Weapon startingWeapon;

    [Space(10)]
    [SerializeField] private GameObject UpgradeUI;
    [SerializeField] private TMPro.TMP_Text upgradeDescriptionTextL;
    [SerializeField] private TMPro.TMP_Text upgradeDescriptionTextR;
    private Upgrade LeftUpgrade;
    private Upgrade RightUpgrade;

    private List<Upgrade> availableUpgrades = new()
    {
        new("Increase Max Health by 5", Upgrade.UpgradeType.HealthIncrease),
        new("Increase Max Ammo by 2", Upgrade.UpgradeType.MaxAmmoIncrease),
        new("Decrease Ammo Regen Time by 10%", Upgrade.UpgradeType.AmmoRegenTimeDecrease),
        new("Increase Damage by 25%", Upgrade.UpgradeType.DamageIncrease),
        new("Decrease Cooldown by 20%", Upgrade.UpgradeType.CooldownDecrease),
        new("Increase Projectile Speed by 10%", Upgrade.UpgradeType.SpeedIncrease),
        new("Increase Projectile Hits by 1", Upgrade.UpgradeType.ProjectileHitsIncrease)
    };

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpgradeUI.SetActive(false);
        CurrentState = GameState.Combat;
        Level = 1;
        Player.Instance.OnStart(startingWeapon);
        enemyPool = new List<GameObject>{ enemyPrefabs[0].Value };
        enemyPrefabs.RemoveAt(0);

        StartCoroutine(SpawnCoroutine());
    }
    
    private void OnEnable()
    {
        InputController.OnButtonLeftPressedEvent += SelectUpgradeLeft;
        InputController.OnButtonRightPressedEvent += SelectUpgradeRight;
    }

    private void OnDisable()
    {
        InputController.OnButtonLeftPressedEvent -= SelectUpgradeLeft;
        InputController.OnButtonRightPressedEvent -= SelectUpgradeRight;
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

    public void ShowUpgradeOptions()
    {
        int leftUpgradeIdx = Random.Range(0, availableUpgrades.Count);
        LeftUpgrade = availableUpgrades[leftUpgradeIdx];
        upgradeDescriptionTextL.text = LeftUpgrade.Description;

        // prevent selecting the same upgrade twice
        availableUpgrades.RemoveAt(leftUpgradeIdx);

        RightUpgrade = availableUpgrades[Random.Range(0, availableUpgrades.Count)];
        upgradeDescriptionTextR.text = RightUpgrade.Description;

        // restore the removed upgrade back to the pool
        availableUpgrades.Add(LeftUpgrade);

        CurrentState = GameState.Upgrade;
        UpgradeUI.SetActive(true);
        Time.timeScale = 0f;
    }

    private void SelectUpgradeLeft(InputController controller)
    {
        if (!CurrentState.Equals(GameState.Upgrade)) return;

        HandleSelectUpgrade(LeftUpgrade);
    }

    private void SelectUpgradeRight(InputController controller)
    {
        if (!CurrentState.Equals(GameState.Upgrade)) return;

        HandleSelectUpgrade(RightUpgrade);
    }

    public void HandleSelectUpgrade(Upgrade selectedUpgrade)
    {
        selectedUpgrade?.ApplyEffect();
        Level++;
        Player.Instance.PlayerLevelUp();
        UpgradeUI.SetActive(false);
        CurrentState = GameState.Combat;
        Time.timeScale = 1f;
    }

    private int GetExpForLevel(int level) { return level * 5; }
    private float GetEnemySpeedMultiplier(int level) { return 2 + level * 0.1f; }
    private (float, float) GetEnemySpawnIntervalRange(int level)
    {
        float min = Mathf.Max(0.5f, 1.5f - level * 0.1f);
        float max = Mathf.Max(1.0f, 3.0f - level * 0.2f);
        return (min, max);
    }
}
