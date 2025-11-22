public class Upgrade
{
    public enum UpgradeType
    {
        HealthIncrease,
        MaxAmmoIncrease,
        AmmoRegenTimeDecrease,
        DamageIncrease,
        CooldownDecrease,
        SpeedIncrease,
        ProjectileHitsIncrease
    }

    public string Description;
    public UpgradeType Type;
    public System.Action ApplyEffect;

    public Upgrade(string description, UpgradeType type)
    {
        Description = description;
        Type = type;
        ApplyEffect = GetAction(type);
    }

    private System.Action GetAction(UpgradeType type)
    {
        return type switch
        {
            UpgradeType.HealthIncrease => () => Player.Instance.IncreaseMaxHealth(5),
            UpgradeType.MaxAmmoIncrease => () => Player.Instance.IncreaseMaxAmmo(2),
            UpgradeType.AmmoRegenTimeDecrease => () => Player.Instance.DecreaseAmmoRegenTime(0.9f),
            UpgradeType.DamageIncrease => () => Player.Instance.IncreaseDamage(1.25f),
            UpgradeType.CooldownDecrease => () => Player.Instance.DecreaseCooldown(0.8f),
            UpgradeType.SpeedIncrease => () => Player.Instance.IncreaseSpeed(1.1f),
            UpgradeType.ProjectileHitsIncrease => () => Player.Instance.IncreaseProjectileHits(1),
            _ => null,
        };
    }
}
