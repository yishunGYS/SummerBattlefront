using Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Systems.Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    public class PlayerStats : MonoBehaviour
    {
        public static PlayerStats Instance;

        [ShowInInspector]
        [HideInInspector]public static float Money = 0;
        public int startMoney = 400;

        //public static int Lives;
        public int startLives = 20;

        //public static int Rounds;

        [Header("当前回复速率")]
        [HideInInspector]public float currentRegainRate;

        [Header("当前的上限")]
        [HideInInspector]public int currentLimit;

        [Header("回复速度")]
        public List<float> regainPhase;

        [Header("资源上限")]
        public List<int> limitPhase;

        [Header("关卡时间限制（秒）")]
        private float levelTimeLimit = 300f;

        private float remainingTime;
        private float switchTimer;
        private float regainTimer;
        private int currentPhaseIndex;

        [ShowInInspector]
        private bool isLevelStarted = false;

        [Header("时间显示组件")]
        public TextMeshProUGUI timeText;

        public bool isEnterEnd = false;

        [Header("时间文本预制件")]
        public GameObject timeTextPrefab;

        private float regainTimeScale = 1f;

        void Awake()
        {
            // 初始化实例
            Instance = this;
        }

        public void OnLevelStart()
        {
            isEnterEnd = false;
            isLevelStarted = false;

            GameObject canvasObject = new GameObject("TimeTextCanvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();

            if (timeTextPrefab != null)
            {
                GameObject timeTextInstance = Instantiate(timeTextPrefab, canvasObject.transform);
                timeText = timeTextInstance.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("Time Text Prefab 未设置！");
            }

            // 获取关卡时间限制
            if (LevelManager.Instance != null)
            {
                levelTimeLimit = LevelManager.Instance.GetCurrentLevelTime();
            }

            Money = startMoney;
            //Lives = startLives;

            if (regainPhase != null && limitPhase != null && regainPhase.Count > 0 && limitPhase.Count > 0)
            {
                currentPhaseIndex = 0;
                currentLimit = limitPhase[currentPhaseIndex];
                currentRegainRate = regainPhase[currentPhaseIndex];
            }
            else
            {
                Debug.LogError("没有设置回复速度和上限");
            }

            //Rounds = 0;
            switchTimer = 0f;
            regainTimer = 0f;
        }

        void FixedUpdate()
        {
            if (isLevelStarted)
            {
                RegainMoneyOverTime();
                UpdateLevelTime();
            }
        }

        public void StartLevel()
        {
            remainingTime = levelTimeLimit;
            isLevelStarted = true;

            Debug.Log("关卡开始！");
        }

        void RegainMoneyOverTime()
        {
            regainTimer += Time.fixedDeltaTime * regainTimeScale;

            if (regainTimer >= 1f)
            {
                Money += currentRegainRate;
                regainTimer = 0f;

                if (Money > currentLimit)
                {
                    Money = currentLimit;
                }
            }
        }

        void UpdateLevelTime()
        {
            remainingTime -= Time.fixedDeltaTime;

            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                isLevelStarted = false;
                UIManager.Instance.OpenLevelFailPanel();
                Debug.Log("关卡失败：时间耗尽！");
            }

            UpdateTimeText();
        }

        void UpdateTimeText()
        {
            if (timeText != null)
            {
                timeText.text = $"Time Left: {remainingTime:F2} s";
            }
        }

        public void CheckVictoryCondition()
        {
            if (isEnterEnd && remainingTime > 0f)
            {
                isLevelStarted = false;
                Debug.Log("关卡成功！");
                Time.timeScale = 0;
                UIManager.Instance.OpenEndLevelPanel();
            }
        }

        public void GainMoney(float num)
        {
            Money += num;
        }

        public void SetRaginTimeScale(float num)
        {
            regainTimeScale = num;
        }

        public void RiseRagineRate(float num)
        {
            currentRegainRate += num;
        }

        public void RiseLimit(int num)
        {
            currentLimit += num;
        }

        public float CurrentMoney()
        {
            return Money;
        }

        public void ResetPlayerStats()
        {
            Money = startMoney;
            //Lives = startLives;
            //Rounds = 0;

            if (regainPhase != null && limitPhase != null && regainPhase.Count > 0 && limitPhase.Count > 0)
            {
                currentPhaseIndex = 0;
                currentLimit = limitPhase[currentPhaseIndex];
                currentRegainRate = regainPhase[currentPhaseIndex];
            }
            else
            {
                Debug.LogError("没有设置回复速度和上限");
            }

            regainTimer = 0f;
            switchTimer = 0f;
            isLevelStarted = false;
            remainingTime = levelTimeLimit;
            regainTimeScale = 1f;
        }
    }
}
