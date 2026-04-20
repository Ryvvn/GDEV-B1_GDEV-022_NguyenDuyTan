using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 10f;

        private Vector3 target;
        private Vector3 direction;
        [SerializeField] float shootInterval = 0.2f;
        private float lastShoot;
        private bool canShoot = true;


        [SerializeField] private GameObject Bullet;
        private void Update()
        {
            if(target != null && target != Vector3.zero)
            {
                MoveTowardTarget(target);
            }
            if(Input.GetMouseButtonDown(0))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    target.z = transform.position.z;
                }
            }
            lastShoot += Time.deltaTime;
            if (lastShoot < shootInterval)
            {
                canShoot = true;
            }
            else
            {
                canShoot = false;
            }

            if(Input.GetKeyDown("space") && canShoot)
            {
                direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                direction.z = transform.position.z;
                Shoot(direction);
            }
        }

      
        private void MoveTowardTarget(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (target != null)
            {
                if (transform.position.x - target.x <= 0.1f && transform.position.y - target.y < 0.1f)
                {
                    target = Vector3.zero;
                }
            }
        }
        private void Shoot(Vector3 target)
        {
          
            GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);

            bullet.GetComponent<Rigidbody2D>().velocity = target;
            Destroy(bullet,3f);

            lastShoot = 0;
            
        }
    }
}