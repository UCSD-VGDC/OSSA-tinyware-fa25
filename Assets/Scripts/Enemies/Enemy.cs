using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    protected const int PLAYER_LAYER = 6;

    public EventReference dmgSFXRef;

    public int Size = 0;

    public float Health = 1f;
    public float Damage = 1f;
    public float MoveSpeed = 1f;
    public int ExperienceReward = 1;
    [SerializeField] protected GameObject visuals;

    [SerializeField] private EventInstance eventInstance;

    protected float liveMoveSpeed;
    protected Rigidbody2D rb;
    protected int moveDirection;
    protected Animator animator;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        liveMoveSpeed = MoveSpeed * GameManager.Instance.EnemySpeedMultiplier;
        moveDirection = (transform.position.x > 0) ? -1 : 1;
        visuals.transform.localScale = new Vector3(moveDirection, 1, 1);
        rb.linearVelocity = new Vector2(liveMoveSpeed * moveDirection, 0);
        animator = GetComponent<Animator>();
        animator.SetFloat("speed", GameManager.Instance.EnemySpeedMultiplier - 1.15f);
        eventInstance = RuntimeManager.CreateInstance(dmgSFXRef);
        eventInstance.setParameterByName("Size", Size);
    }

    public virtual void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0)
        {
            // Enemy defeated
            Player.Instance.GainExperience(ExperienceReward);
            Player.Instance.TryApplyRandomUpgrades();
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == PLAYER_LAYER)
        {
            eventInstance.start();
            Player.Instance.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
