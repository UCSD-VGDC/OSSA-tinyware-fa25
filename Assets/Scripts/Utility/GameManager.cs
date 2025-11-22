using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int Level = 1;
    public int ExpNeeded => GetExpForLevel(Level);

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
}
