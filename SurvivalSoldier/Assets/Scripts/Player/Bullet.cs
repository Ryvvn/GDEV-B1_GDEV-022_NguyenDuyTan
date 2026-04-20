using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    [SerializeField] private LayerMask bounceLayerMask;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>() == null)
        {
          Destroy(gameObject);
        }

        if(bounceLayerMask == (bounceLayerMask | (1 << collision.gameObject.layer)))
        {
            Vector2 normal = collision.contacts[0].normal;
            Vector2 incomingVelocity = GetComponent<Rigidbody2D>().velocity;
            Vector2 reflectedVelocity = Vector2.Reflect(incomingVelocity, normal);
            GetComponent<Rigidbody2D>().velocity = reflectedVelocity;
        }
    }
}
