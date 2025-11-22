using UnityEngine;

[CreateAssetMenu(fileName = "New TestBlaster", menuName = "Weapons/TestBlaster")]
public class TestBlaster : Weapon
{
    public override void Attack(int direction)
    {
        Debug.Log($"TestBlaster fired to the {(direction > 0 ? "right" : "left")}!");
        GameObject projectileObj = Instantiate(projectilePrefab, Player.Instance.transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Instantiate(ProjectileSpeed, Damage, direction, ProjectileHits);
    }
}
