using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private const int PLAYER_LAYER = 6;

    public float Health = 1f;
    public float Damage = 1f;
    public float MoveSpeed = 1f;
    public int ExperienceReward = 1;
    [SerializeField] private GameObject visuals;

    private float liveMoveSpeed;
    private Rigidbody2D rb;
    private int moveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        liveMoveSpeed = MoveSpeed * GameManager.Instance.EnemySpeedMultiplier;
        moveDirection = (transform.position.x > 0) ? -1 : 1;
        visuals.transform.localScale = new Vector3(moveDirection, 1, 1);
        rb.linearVelocity = new Vector2(liveMoveSpeed * moveDirection, 0);
    }

    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0)
        {
            // Enemy defeated
            Player.Instance.GainExperience(ExperienceReward);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == PLAYER_LAYER)
        {
            Player.Instance.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
