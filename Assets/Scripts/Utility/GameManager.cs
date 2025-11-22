using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Combat,
        Upgrade,
        NoInput,
        DeathScreen
    }

    public static GameManager Instance;
    public GameState CurrentState { get; private set; } = GameState.Combat;

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
    // public float EnemySpeedMultiplier => GetEnemySpeedMultiplier(Level);
    public float EnemySpeedMultiplier = 2.5f;
    public (float,float) EnemySpawnIntervalRange => GetEnemySpawnIntervalRange(Level);
    [SerializeField] private List<MyKeyValuePair<int, GameObject>> enemyPrefabs;
    [SerializeField] private List<GameObject> enemySpawnPoints;
    private List<GameObject> enemyPool = new();
    [SerializeField] private Weapon startingWeapon;
    [SerializeField] private TMPro.TMP_Text levelText;

    [Space(10)]
    [SerializeField] private GameObject UpgradeUI;
    [SerializeField] private RectTransform upgradeBoxL;
    [SerializeField] private RectTransform upgradeBoxR;
    [SerializeField] private TMPro.TMP_Text upgradeDescriptionTextL;
    [SerializeField] private TMPro.TMP_Text upgradeDescriptionTextR;
    [SerializeField] private RerollButton rerollButton;
    private Upgrade LeftUpgrade;
    private Upgrade RightUpgrade;
    private bool canReroll = true;

    [Space(10)]
    [SerializeField] private GameObject CombatOnlyUI;

    [Space(10)]
    [SerializeField] private GameObject DeathUI;

    private List<Upgrade> availableUpgrades = new()
    {
        new("+3 Health", Upgrade.UpgradeType.Heal),
        new("+5 Max Health", Upgrade.UpgradeType.MaxHealthIncrease),
        new("+2 Max Ammo", Upgrade.UpgradeType.MaxAmmoIncrease),
        new("-25% Ammo Regen Time", Upgrade.UpgradeType.AmmoRegenTimeDecrease),
        new("+25% Damage", Upgrade.UpgradeType.DamageIncrease),
        new("-15% Attack Cooldown", Upgrade.UpgradeType.CooldownDecrease),
        new("+10% Projectile Speed", Upgrade.UpgradeType.SpeedIncrease),
        new("+1 Enemies Hit per Projectile", Upgrade.UpgradeType.ProjectileHitsIncrease),
        new("+10% Chance for +1 Health on Kill", Upgrade.UpgradeType.RandomHealChance),
        new("+10% Chance for +1 Ammo on Kill", Upgrade.UpgradeType.RandomAmmoChance),
    };

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        CombatOnlyUI.SetActive(true);
        UpgradeUI.SetActive(false);
        DeathUI.SetActive(false);
        CurrentState = GameState.Combat;
        Level = 1;
        Player.Instance.OnStart(startingWeapon);
        enemyPool = new List<GameObject>{ enemyPrefabs[0].Value };
        enemyPrefabs.RemoveAt(0);

        StartCoroutine(SpawnCoroutine());
    }
    
    private void OnEnable()
    {
        InputController.OnButtonLeftPressedEvent += MenuButtonLeft;
        InputController.OnButtonRightPressedEvent += MenuButtonRight;
        InputController.OnBothButtonsPressedEvent += MenuButtonBoth;
    }

    private void OnDisable()
    {
        InputController.OnButtonLeftPressedEvent -= MenuButtonLeft;
        InputController.OnButtonRightPressedEvent -= MenuButtonRight;
        InputController.OnBothButtonsPressedEvent -= MenuButtonBoth;
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(EnemySpawnIntervalRange.Item1, EnemySpawnIntervalRange.Item2);
            yield return new WaitForSeconds(waitTime);

            int randomIndex = UnityEngine.Random.Range(0, enemyPool.Count);
            GameObject enemyPrefab = enemyPool[randomIndex];
            Vector3 spawnPosition = enemySpawnPoints[UnityEngine.Random.Range(0, enemySpawnPoints.Count)].transform.position;
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void ShowUpgradeOptions()
    {
        Debug.Log("Showing upgrade options");
        int leftUpgradeIdx = -1;
        if (Player.Instance.Health < Player.Instance.MaxHealth && canReroll)
        {
            // ensure Heal upgrade is available if player is not at full health before rerolling
            leftUpgradeIdx = 0;
        }
        else
        {
            leftUpgradeIdx = UnityEngine.Random.Range(1, availableUpgrades.Count);
        }
        LeftUpgrade = availableUpgrades[leftUpgradeIdx];
        upgradeDescriptionTextL.text = LeftUpgrade.Description;

        // prevent selecting the same upgrade twice
        availableUpgrades.RemoveAt(leftUpgradeIdx);

        int rightMinIdx = Player.Instance.Health < Player.Instance.MaxHealth ? 0 : 1;
        RightUpgrade = availableUpgrades[UnityEngine.Random.Range(rightMinIdx, availableUpgrades.Count)];
        upgradeDescriptionTextR.text = RightUpgrade.Description;

        // restore the removed upgrade back to the pool
        availableUpgrades.Insert(leftUpgradeIdx, LeftUpgrade);

        CurrentState = GameState.NoInput;
        CombatOnlyUI.SetActive(false);
        UpgradeUI.SetActive(true);
        Time.timeScale = 0f;

        float targetY = -25f;

        StartCoroutine(Tweens.InterpolateRealTime(
            null,
            (t) =>
            {
                float newY = Tweens.EaseOutBack(targetY - 150f, targetY, t);
                upgradeBoxL.anchoredPosition = new Vector2(upgradeBoxL.anchoredPosition.x, newY);
                upgradeBoxR.anchoredPosition = new Vector2(upgradeBoxR.anchoredPosition.x, newY);
            },
            () => { CurrentState = GameState.Upgrade; },
            0.5f
        ));
    }

    private void MenuButtonLeft(InputController controller)
    {
        if (CurrentState.Equals(GameState.Upgrade)) HandleSelectUpgrade(LeftUpgrade);
        else if (CurrentState.Equals(GameState.DeathScreen))
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    private void MenuButtonRight(InputController controller)
    {
        if (CurrentState.Equals(GameState.Upgrade)) HandleSelectUpgrade(RightUpgrade);
        else if (CurrentState.Equals(GameState.DeathScreen))
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    private void MenuButtonBoth(InputController controller)
    {
        if (!CurrentState.Equals(GameState.Upgrade) || !canReroll) return;

        canReroll = false;
        rerollButton.ToggleEnabled(false);

        float startY = -25f;

        StartCoroutine(Tweens.InterpolateRealTime(
            null,
            (t) =>
            {
                float newY = Tweens.EaseInBack(startY, startY - 150f, t);
                upgradeBoxL.anchoredPosition = new Vector2(upgradeBoxL.anchoredPosition.x, newY);
                upgradeBoxR.anchoredPosition = new Vector2(upgradeBoxR.anchoredPosition.x, newY);
            },
            () => { Debug.Log("Finishing tween"); ShowUpgradeOptions(); },
            0.3f
        ));
    }

    public void HandleSelectUpgrade(Upgrade selectedUpgrade)
    {
        selectedUpgrade?.ApplyEffect();
        Level++;
        levelText.text = $"Level {Level}";
        Player.Instance.PlayerLevelUp();
        UpgradeUI.SetActive(false);
        canReroll = true;
        rerollButton.ToggleEnabled(true);
        CombatOnlyUI.SetActive(true);
        CurrentState = GameState.Combat;
        Time.timeScale = 1f;
    }

    public void HandlePlayerDeath()
    {
        CurrentState = GameState.NoInput;
        Time.timeScale = 0f;
        CombatOnlyUI.SetActive(false);
        UpgradeUI.SetActive(false);
        DeathUI.SetActive(true);
        CurrentState = GameState.DeathScreen;
    }

    private int GetExpForLevel(int level) { return level * 5; }
    // private float GetEnemySpeedMultiplier(int level) { return 2 + level * 0.15f; }
    private (float, float) GetEnemySpawnIntervalRange(int level)
    {
        float min = 2f * Mathf.Exp(level / 4f * -1f);
        float max = 4f * Mathf.Exp(level / 4f * -1f);
        return (min, max);
    }
}
