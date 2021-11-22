using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voronoi
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float speed = 2000000f;

        private Rigidbody rb;

        // Start is called before the first frame update
        void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            if (moveHorizontal != 0 || moveVertical != 0)
            {
                Vector3 movement = ((transform.forward * moveVertical) +
                                   (transform.right * moveVertical)).normalized;

                Debug.Log(movement.magnitude);
                rb.AddForce(speed * movement);
            }
        }


    }
}