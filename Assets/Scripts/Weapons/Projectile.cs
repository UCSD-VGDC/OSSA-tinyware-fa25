using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private int enemyHits;

    public void Instantiate(float speed, float damage, int direction, int enemyHits)
    {
        this.damage = damage;
        this.enemyHits = enemyHits;

        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(speed * direction, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == Player.ENEMY_LAYER)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage);
            enemyHits--;
            if (enemyHits <= 0) Destroy(gameObject);
        }
    }
}
