using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public GameObject projectilePrefab;
    public float Damage;
    public int MaxAmmo;
    public float AmmoRegenTime;
    public float Cooldown;
    public float ProjectileSpeed;
    public int ProjectileHits;

    public abstract void Attack(int direction);
}
