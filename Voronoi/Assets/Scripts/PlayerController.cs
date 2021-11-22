using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voronoi
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private CharacterController controller;

        [SerializeField]
        private Transform cam;

        public float moveSpeed = 4f;
        public float jumpSpeed = 2f;

        public float turnSmoothTime = 0.1f;
        private float turnSmoothVelocity;

        private float gravity = 9.8f;

        private float vSpeed = 0;

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.05f || vSpeed != 0)
            {
                // Rotate
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(
                    transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

                Vector3 moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;

                // Jump and gravity
                if (controller.isGrounded)
                {
                    vSpeed = 0; // grounded character has vSpeed = 0...
                    if (Input.GetKeyDown(KeyCode.Space))
                    { // unless it jumps:
                        vSpeed = jumpSpeed;
                    }
                }
                // apply gravity acceleration to vertical speed:
                vSpeed -= gravity * Time.deltaTime;
                moveDir.y = vSpeed; // include vertical speed in vel

                controller.Move(moveSpeed * Time.deltaTime * moveDir);
            }

        }

    }
}