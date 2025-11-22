using UnityEngine;

[CreateAssetMenu(fileName = "New TestBlaster", menuName = "Weapons/TestBlaster")]
public class TestBlaster : Weapon
{
    public override void Attack(int direction)
    {
        Debug.Log($"TestBlaster fired to the {(direction > 0 ? "right" : "left")}!");
        // instantiate projectile in facing direction with currentWeapon.ProjectileSpeed and currentWeapon.Damage
    }
}
