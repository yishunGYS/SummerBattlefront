using Managers;
using Sirenix.OdinInspector;
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
        public static float Money;
        public int startMoney = 400;

        public static int Lives;
        public int startLives = 20;

        [Header("当前回复速率")]
        public float currentRegainRate = 5f;

        [Header("当前的上限")]
        public int currentLimit = 100;

        [Header("关卡时间限制（秒）")]
        private float levelTimeLimit = 300f;

        private float remainingTime;
        private float regainTimer;

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
            Lives = startLives;
            regainTimer = 0f;
        }

        void FixedUpdate()
        {
            RegainMoneyOverTime();

            if (isLevelStarted)
            {
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
                timeText = null;
                Debug.Log("关卡成功！");
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
            Lives = startLives;
            regainTimer = 0f;
            isLevelStarted = false;
            remainingTime = levelTimeLimit;
            regainTimeScale = 1f;
        }
    }
}
