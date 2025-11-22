using UnityEngine;

[CreateAssetMenu(fileName = "New TestBlaster", menuName = "Weapons/TestBlaster")]
public class TestBlaster : Weapon
{
    public override void Attack(int direction)
    {
        GameObject projectileObj = Instantiate(projectilePrefab, Player.Instance.ProjectileSpawnPoint.transform.position, Quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Instantiate(ProjectileSpeed, Damage, direction, ProjectileHits);
    }
}
