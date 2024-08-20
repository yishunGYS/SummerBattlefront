using Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Systems.Level;
using TMPro; // 引入TextMeshPro的命名空间
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    public class PlayerStats : MonoBehaviour
    {
        public static PlayerStats Instance;

        [ShowInInspector]
        public static int Money;
        public int startMoney = 400;

        public static int Lives;
        public int startLives = 20;

        public static int Rounds;

        [Header("当前回复速率")]
        public int currentRegainRate;

        [Header("当前的上限")]
        public int currentLimit;

        //[Header("经过多长时间切换到下一个阶段")]
        //public int switchPhase;

        [Header("回复速度")]
        public List<int> regainPhase;

        [Header("资源上限")]
        public List<int> limitPhase;

        [Header("关卡时间限制（秒）")]
        public float levelTimeLimit = 300f;

        private float remainingTime; // 剩余时间
        private float switchTimer;
        private float regainTimer;
        private int currentPhaseIndex;

        private bool isLevelStarted = false;

        // 定义OnMoneyChanged事件
        public static event Action<int, int> OnMoneyChanged;

        // TextMeshPro组件用于显示剩余时间
        [Header("时间显示组件")]
        public TextMeshProUGUI timeText;

        //[HideInInspector]
        public bool isEnterEnd = false;

        // 你可以在这里指定时间文本预制件
        [Header("时间文本预制件")]
        public GameObject timeTextPrefab;

        private float regainTimeScale = 1f;

        void Awake()
        {
            // 初始化实例
            Instance = this;
        }

        void Start()
        {
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
            Lives = startLives;

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

            Rounds = 0;
            switchTimer = 0f;
            regainTimer = 0f;

            // 初始更新UI
            OnMoneyChanged?.Invoke(Money, currentLimit);
            UpdateTimeText(); // 初始时间显示
        }

        void FixedUpdate()
        {
            if (isLevelStarted)
            {
                RegainMoneyOverTime();
                UpdateLevelTime(); // 更新关卡时间
                //SwitchPhaseOverTime();
            }
        }

        public void StartLevel()
        {
            // 初始化时间和状态
            remainingTime = levelTimeLimit;
            isLevelStarted = true;

            UpdateTimeText();

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

                // 触发OnMoneyChanged事件
                OnMoneyChanged?.Invoke(Money, currentLimit);
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

            UpdateTimeText(); // 更新时间显示
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


        public void GainMoney(int num)
        {
            Money += num;
        }
        public void SetRaginTimeScale(float num)
        {
            regainTimeScale = num;
        }

        public void RiseRagineRate(int num)
        {
            currentRegainRate += num;
        }

        public void RiseLimit(int num)
        {
            currentLimit += num;
        }

        public int CurrentMoney()
        {
            return Money;
        }
    }
}
