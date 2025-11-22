public class Upgrade
{
    public enum UpgradeType
    {
        Heal,
        MaxHealthIncrease,
        MaxAmmoIncrease,
        AmmoRegenTimeDecrease,
        DamageIncrease,
        CooldownDecrease,
        SpeedIncrease,
        ProjectileHitsIncrease,
        RandomHealChance,
        RandomAmmoChance,
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
            UpgradeType.Heal => () => Player.Instance.TakeDamage(-3),
            UpgradeType.MaxHealthIncrease => () => Player.Instance.IncreaseMaxHealth(5),
            UpgradeType.MaxAmmoIncrease => () => Player.Instance.IncreaseMaxAmmo(2),
            UpgradeType.AmmoRegenTimeDecrease => () => Player.Instance.DecreaseAmmoRegenTime(0.75f),
            UpgradeType.DamageIncrease => () => Player.Instance.IncreaseDamage(1.25f),
            UpgradeType.CooldownDecrease => () => Player.Instance.DecreaseCooldown(0.85f),
            UpgradeType.SpeedIncrease => () => Player.Instance.IncreaseSpeed(1.1f),
            UpgradeType.ProjectileHitsIncrease => () => Player.Instance.IncreaseProjectileHits(1),
            UpgradeType.RandomHealChance => () => Player.Instance.IncreaseRandomHealChance(0.1f),
            UpgradeType.RandomAmmoChance => () => Player.Instance.IncreaseRandomAmmoChance(0.1f),
            _ => null,
        };
    }
}
