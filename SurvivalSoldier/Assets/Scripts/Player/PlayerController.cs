using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 10f;

        private Vector3 target;
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
    }
}