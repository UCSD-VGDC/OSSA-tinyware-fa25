using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    public static Player Instance;
    public const int ENEMY_LAYER = 7;

    [SerializeField] private float maxHealth = 5f;
    [SerializeField] private GameObject visuals;

    [Space(10)]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image ammoBar;
    [SerializeField] private Image expBar;

    private float health;
    public float Health
    {
        get { return health; }
        private set
        {
            health = value;
            healthBar.fillAmount = health / maxHealth;
        }
    }

    private int ammo;
    public int Ammo
    {
        get { return ammo; }
        private set
        {
            ammo = value;
            ammoBar.fillAmount = (float)ammo / currentWeapon.MaxAmmo;
        }
    }

    private int experience;
    public int Experience
    {
        get { return experience; }
        private set
        {
            experience = value;
            expBar.fillAmount = (float)experience / GameManager.Instance.ExpNeeded; // Assuming 100 EXP for next level
        }
    }

    private Weapon currentWeapon;
    private bool canAttack = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OnStart(Weapon startingWeapon)
    {
        Health = maxHealth;
        Experience = 0;
        EquipWeapon(startingWeapon);
    }
    
    private void OnEnable()
    {
        InputController.OnButtonLeftPressedEvent += AttackLeft;
        InputController.OnButtonRightPressedEvent += AttackRight;
    }

    private void OnDisable()
    {
        InputController.OnButtonLeftPressedEvent -= AttackLeft;
        InputController.OnButtonRightPressedEvent -= AttackRight;
    }

    private void Update()
    {
        // TODO: REMOVE DEBUG
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GainExperience(1);
        }
    }

    private void AttackLeft(InputController controller)
    {
        visuals.transform.localScale = new Vector3(-1, 1, 1);

        if (!GameManager.Instance.CurrentState.Equals(GameManager.GameState.Combat) || !canAttack || Ammo <= 0)
        {
            // TODO: play no ammo sfx
            return;
        }

        HandleAttack();
    }

    private void AttackRight(InputController controller)
    {
        visuals.transform.localScale = new Vector3(1, 1, 1);

        if (!GameManager.Instance.CurrentState.Equals(GameManager.GameState.Combat) || !canAttack || Ammo <= 0)
        {
            // TODO: play no ammo sfx
            return;
        }

        HandleAttack();
    }

    private void HandleAttack()
    {
        currentWeapon.Attack(visuals.transform.localScale.x > 0 ? 1 : -1);
        Ammo--;
        // TODO: play attack sfx
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(currentWeapon.Cooldown);
        canAttack = true;
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        Ammo = newWeapon.MaxAmmo;
    }

    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
        if (Health < 0) Health = 0;
        Debug.Log($"Player took damage: {damageAmount}, Current Health: {Health}");
        healthBar.fillAmount = Health / maxHealth;
    }

    public void GainExperience(int expAmount)
    {
        Experience += expAmount;
        if (Experience >= GameManager.Instance.ExpNeeded)
        {
            // handle level up
            Debug.Log($"Level Up: {GameManager.Instance.Level} -> {GameManager.Instance.Level + 1}, EXP Needed: {GameManager.Instance.ExpNeeded}");

            GameManager.Instance.ShowUpgradeOptions();
        }
    }

    public void PlayerLevelUp()
    {
        Experience = 0;
        Health = maxHealth;
        Ammo = currentWeapon.MaxAmmo;
    }

    public void IncreaseMaxHealth(int amount) { maxHealth += amount; }
    public void IncreaseMaxAmmo(int amount) { currentWeapon.MaxAmmo += amount; }
    public void DecreaseAmmoRegenTime(float multiplier) { currentWeapon.AmmoRegenTime *= multiplier; }
    public void IncreaseDamage(float multiplier) { currentWeapon.Damage *= multiplier; }
    public void DecreaseCooldown(float multiplier) { currentWeapon.Cooldown *= multiplier; }
    public void IncreaseSpeed(float multiplier) { currentWeapon.ProjectileSpeed *= multiplier; }
    public void IncreaseProjectileHits(int amount) { currentWeapon.ProjectileHits += amount; }
}
