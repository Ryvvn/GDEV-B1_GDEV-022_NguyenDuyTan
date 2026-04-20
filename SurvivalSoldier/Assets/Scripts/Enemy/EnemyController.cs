using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int hp = 1;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float directionChangeInterval = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float chaseSpeedMultiplier = 1.2f;

    private Transform playerTarget;
    private Vector2 moveDirection;
    private float directionTimer;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
        }

        PickRandomDirection();
        directionTimer = directionChangeInterval;
    }

    private void Update()
    {
        bool shouldChase = ShouldChasePlayer();

        if (shouldChase)
        {
            Vector2 towardPlayer = (playerTarget.position - transform.position).normalized;
            moveDirection = towardPlayer;
            transform.position += (Vector3)(moveDirection * (moveSpeed * chaseSpeedMultiplier) * Time.deltaTime);
            return;
        }

        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0f)
        {
            PickRandomDirection();
            directionTimer = directionChangeInterval;
        }

        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            hp -= bullet.damage;
            Destroy(collision.gameObject);

            if (hp <= 0)
            {
                Destroy(gameObject);
            }

            return;
        }

        Health playerHealth = collision.gameObject.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }
    }

    private bool ShouldChasePlayer()
    {
        if (playerTarget == null)
        {
            return false;
        }

        return Vector2.Distance(transform.position, playerTarget.position) <= detectionRange;
    }

    private void PickRandomDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;
        if (moveDirection == Vector2.zero)
        {
            moveDirection = Vector2.right;
        }
    }
}



