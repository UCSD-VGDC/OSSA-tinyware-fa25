using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    private int phasesRemaining = 3;
    private float phaseMaxHealth = 5f;
    private float phaseDelaySeconds = 4f;
    private float retreatPositionX;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        int bossRank = GameManager.Instance.Level / 5;
        Damage += bossRank;
        phaseMaxHealth += bossRank;
        phaseDelaySeconds *= 1f - (bossRank * 0.1f);
        BeginPhase();
    }

    private void BeginPhase()
    {
        float startPosX = (Random.value < 0.5f) ? -13.5f : 13.5f;
        Vector3 startPosition = new(startPosX, transform.position.y, transform.position.z);
        transform.position = startPosition;

        liveMoveSpeed = MoveSpeed * GameManager.Instance.EnemySpeedMultiplier;
        moveDirection = (transform.position.x > 0) ? -1 : 1;
        visuals.transform.localScale = new Vector3(moveDirection, 1, 1);
        rb.linearVelocity = new Vector2(liveMoveSpeed * moveDirection, 0);
    }

    public override void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
        GameManager.Instance.UpdateBossHealthBar(((phasesRemaining - 1) * phaseMaxHealth + Health) / (3 * phaseMaxHealth));
        if (Health <= 0)
        {
            phasesRemaining--;
            if (phasesRemaining > 0)
            {
                Health = phaseMaxHealth;
                Retreat();
            }
            else
            {
                Destroy(gameObject);
                GameManager.Instance.UpgradesToGain = 3;
                GameManager.Instance.ShowUpgradeOptions();
            }
        }
    }

    private void Retreat()
    {
        retreatPositionX = (moveDirection == 1) ? -13.5f : 13.5f;
        visuals.transform.localScale = new Vector3(moveDirection * -1, 1, 1);
        rb.linearVelocity = new Vector2(liveMoveSpeed * 2 * moveDirection * -1, 0);
        StartCoroutine(RetreatCoroutine());
    }

    private IEnumerator RetreatCoroutine()
    {
        while (moveDirection == 1 ? transform.position.x > retreatPositionX : transform.position.x < retreatPositionX)
        {
            yield return null;
        }
        yield return new WaitForSeconds(phaseDelaySeconds);
        BeginPhase();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == PLAYER_LAYER)
        {
            Player.Instance.TakeDamage(Damage);
            Retreat();
        }
    }
}
