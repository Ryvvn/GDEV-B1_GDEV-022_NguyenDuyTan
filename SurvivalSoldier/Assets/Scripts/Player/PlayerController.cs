using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float fireRate = 5f;
        [SerializeField] private float bulletSpeed = 14f;
        [SerializeField] private GameObject bulletPrefab;

        private Vector3 target;
        private float shootTimer;

        private void Update()
        {
            if (target != Vector3.zero)
            {
                MoveTowardTarget(target);
            }

            if (Input.GetMouseButtonDown(0))
            {
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.z = transform.position.z;
            }

            shootTimer += Time.deltaTime;
            float shootInterval = 1f / Mathf.Max(0.01f, fireRate);
            bool canShoot = shootTimer >= shootInterval;

            if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(1)) && canShoot)
            {
                Vector3 aimPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                aimPoint.z = transform.position.z;
                Shoot(aimPoint);
            }
        }

        private void MoveTowardTarget(Vector3 moveTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, moveTarget) <= 0.1f)
            {
                target = Vector3.zero;
            }
        }

        private void Shoot(Vector3 aimPoint)
        {
            if (bulletPrefab == null)
            {
                return;
            }

            Vector2 direction = (aimPoint - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.velocity = direction * bulletSpeed;
            }

            Bullet projectile = bullet.GetComponent<Bullet>();
            if (projectile != null)
            {
                projectile.damage = 1;
            }

            Destroy(bullet, 3f);

            shootTimer = 0f;
        }
    }
}