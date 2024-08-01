using System;
using Cinemachine;
using UnityEngine;

namespace Systems.Camera
{
    public class CameraSystem : MonoBehaviour
    {
        public CinemachineVirtualCamera virtualCamera;


        public float dragSpeed;
        public float rotateSpeed;
        public float zoomSpeed;
        public float curFov;
        public float maxFov;
        public float minFov;

        private void Start()
        {
            curFov = virtualCamera.m_Lens.FieldOfView;
        }

        private void Update()
        {
            HandleCameraMove();
            //HandleCameraRotate();

            HandleCameraZoom();
        }

        private void HandleCameraMove()
        {
            var inputDir = new Vector2(0, 0);
            if (Input.GetMouseButton(0))
            {
                inputDir.x += Input.GetAxisRaw("Mouse X");
                inputDir.y += Input.GetAxisRaw("Mouse Y");
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputDir.x += 0.2f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                inputDir.x -= 0.2f;
            }
            if (Input.GetKey(KeyCode.W))
            {
                inputDir.y += 0.2f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                inputDir.y -= 0.2f;
            }
            var moveDir = transform.forward * inputDir.y + transform.right * inputDir.x;

            transform.position += Time.deltaTime * dragSpeed * moveDir;
        }


        private void HandleCameraRotate()
        {
            var rotateDir = 0f;
            if (Input.GetMouseButton(1))
            {
                rotateDir += Input.GetAxisRaw("Mouse X");
                rotateDir -= Input.GetAxisRaw("Mouse Y");
            }

            if (Input.GetKey(KeyCode.Q))
            {
                rotateDir += 0.2f;
            }

            if (Input.GetKey(KeyCode.E))
            {
                rotateDir -= 0.2f;
            }
            transform.eulerAngles += new Vector3(0, Time.deltaTime * rotateSpeed * rotateDir, 0);
        }

        private void HandleCameraZoom()
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                curFov = virtualCamera.m_Lens.FieldOfView + zoomSpeed;
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                curFov = virtualCamera.m_Lens.FieldOfView - zoomSpeed;
            }

            virtualCamera.m_Lens.FieldOfView = Math.Clamp(curFov, minFov, maxFov);
        }
    }
}