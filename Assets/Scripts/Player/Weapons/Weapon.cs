using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public Sprite projectileSprite;
    public abstract void Attack();
}
