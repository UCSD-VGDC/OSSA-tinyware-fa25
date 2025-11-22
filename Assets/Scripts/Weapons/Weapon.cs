using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public Sprite projectileSprite;
    public float Damage;
    public int MaxAmmo;
    public float Cooldown;
    public float ProjectileSpeed;
    public abstract void Attack(int direction);
}
