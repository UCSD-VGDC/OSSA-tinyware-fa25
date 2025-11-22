using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float Health = 1f;
    public float Damage = 1f;
    public float MoveSpeed = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, MoveSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
        if (Health < 0) Health = 0;
    }
}
