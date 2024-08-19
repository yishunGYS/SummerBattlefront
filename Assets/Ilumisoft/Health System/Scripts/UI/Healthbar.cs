using System;
using Gameplay.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Ilumisoft.Health_System.Scripts.UI
{
    [AddComponentMenu("Health System/UI/Healthbar")]
    public class Healthbar : MonoBehaviour
    {
        [SerializeField]
        Canvas canvas;

        [SerializeField]
        Image fillImage;

        [SerializeField, Tooltip("Whether the healthbar should be hidden when health is empty")]
        bool hideEmpty = false;

        [SerializeField, Min(0.1f), Tooltip("Controls how fast changes will be animated in points/second")]
        float changeSpeed = 100;

        [SerializeField, Tooltip("The base angle for the healthbar's x-rotation when close to the camera")]
        float baseAngle = 500f;

        float currentValue;
        private float maxValue;
        private UnitAgent agent;

        public void OnInit(UnitAgent agent)
        {
            this.agent = agent;
            currentValue = agent.GetMaxHp();
            maxValue = agent.GetMaxHp();
        }

        private void LateUpdate()
        {
            if (!agent)
            {
                return;
            }

            // Update the rotation of the Canvas
            SetRotationBasedOnDistance();

            currentValue = Mathf.MoveTowards(currentValue, agent.curHp, Time.deltaTime * changeSpeed);
            UpdateFillbar();
            UpdateVisibility();
        }

        void SetRotationBasedOnDistance()
        {
            if (Camera.main != null)
            {
                // 计算摄像机与Healthbar之间的Z轴位移
                float zOffset = Camera.main.transform.position.z - transform.position.z;

                // 计算距离摄像机的总距离
                float distanceToCamera = Vector3.Distance(Camera.main.transform.position, transform.position);

                // 基于距离计算旋转角度
                float targetXRotation = baseAngle / distanceToCamera;

                // 应用rotation.x，并保持y和z为0
                canvas.transform.rotation = Quaternion.Euler(targetXRotation, 0f, 0f);
            }
        }

        void UpdateFillbar()
        {
            // Update the fill amount
            float value = Mathf.InverseLerp(0, maxValue, currentValue);
            fillImage.fillAmount = value;
        }

        void UpdateVisibility()
        {
            float value = fillImage.fillAmount;

            if (canvas != null)
            {
                // Hide if empty
                if (Mathf.Approximately(value, 0))
                {
                    if (hideEmpty && canvas.gameObject.activeSelf)
                    {
                        canvas.gameObject.SetActive(false);
                    }
                }
                // Make sure the canvas is enabled if health is not empty
                else if (value > 0 && canvas.gameObject.activeSelf == false)
                {
                    canvas.gameObject.SetActive(true);
                }
            }
        }
    }
}
