using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int Level = 1;
    public int ExpNeeded => GetExpForLevel(Level);
    public float EnemySpeedMultiplier => GetEnemySpeedMultiplier(Level);

    [SerializeField] private Weapon startingWeapon;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Player.Instance.OnStart(startingWeapon);
    }

    private int GetExpForLevel(int level) { return level * 5; }
    private float GetEnemySpeedMultiplier(int level) { return 2 + level * 0.1f; }
}
