using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float Health = 1f;
    public float Damage = 1f;
    public float MoveSpeed = 1f;

    private float liveMoveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        liveMoveSpeed = MoveSpeed * GameManager.Instance.EnemySpeedMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, liveMoveSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
        if (Health < 0) Health = 0;
    }
}
