using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voronoi
{
    public class CameraController : MonoBehaviour
    {
        private Camera cam;
        private float panSpeed = 1f;
        private float zoomSpeed = 20f;

        // Start is called before the first frame update
        void OnEnable()
        {
            cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            float hMovement = Input.GetAxisRaw("Horizontal");
            float vMovement = Input.GetAxisRaw("Vertical");

            if (hMovement != 0 || vMovement != 0)
            {
                Vector3 currentPos = cam.transform.position;

                currentPos +=
                    currentPos.y * panSpeed * Time.deltaTime *
                    Vector3.Normalize(new Vector3(hMovement, 0, vMovement));
                cam.transform.position = currentPos;

            }

            float zoom = Input.GetAxisRaw("Mouse ScrollWheel");
            if (zoom != 0)
            {
                Vector3 currentPos = cam.transform.position;
                currentPos +=
                    currentPos.y * Time.deltaTime * -zoomSpeed *
                    Vector3.Normalize(new Vector3(0, zoom, 0));
                cam.transform.position = currentPos;
            }

        }
    }
}