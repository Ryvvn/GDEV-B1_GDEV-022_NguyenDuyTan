using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    [SerializeField] private float wallSeparationAfterBounce = 0.02f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>() != null)
        {
            return;
        }

        int wallLayer = LayerMask.NameToLayer("Wall");
        if (collision.gameObject.layer == wallLayer && collision.contactCount > 0 && rb != null)
        {
            Vector2 normal = collision.contacts[0].normal;
            Vector2 incomingVelocity = collision.relativeVelocity;
            if (incomingVelocity.sqrMagnitude <= 0.0001f)
            {
                incomingVelocity = rb.velocity;
            }

            float speed = incomingVelocity.magnitude;
            Vector2 reflectedDirection = Vector2.Reflect(incomingVelocity.normalized, normal).normalized;
            rb.velocity = reflectedDirection * speed;

            // Nudge out of the wall so the bullet does not remain in contact and slide.
            rb.position += normal * wallSeparationAfterBounce;
            return;
        }

        Destroy(gameObject);
    }
}
