using Managers;
using Sirenix.OdinInspector;
using Systems.Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Systems
{
    public class PlayerStats : Singleton<PlayerStats>
    {
        [ShowInInspector]
        [HideInInspector]public static float Money = 0;
        public int startMoney = 400;

        [Header("当前回复速率")]
        public float currentRegainRate = 5f;

        [Header("当前的上限")]
        public int currentLimit = 100;

        [Header("关卡时间限制（秒）")]
        public float levelTimeLimit = 300f;

        public float remainingTime;
        private float regainTimer;

        [ShowInInspector]
        private bool isLevelStarted = false;

        public bool isEnterEnd = false;

        private float regainTimeScale = 1f;

        public void OnLevelStart()
        {
            // 获取关卡时间限制
            if (LevelManager.Instance != null)
            {
                levelTimeLimit = LevelManager.Instance.GetCurrentLevelTime();
            }
            UIManager.Instance.OnOpenTimeLeftPanel();
            UIManager.Instance.OnOpenResourcePanel();
        }

        void FixedUpdate()
        {
            if (isLevelStarted)
            {
                RegainMoneyOverTime();
                UpdateLevelTime();
                UIManager.Instance.OnUpdateResourcePanel();
            }
        }

        public void StartLevel()
        {
            if (!isLevelStarted)
            {
                remainingTime = levelTimeLimit;
                isLevelStarted = true;
                Debug.Log("关卡开始！");
            }
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
                UIManager.Instance.OnCloseTimeLeftPanel();
                SpawnManager.Instance.isLevelStarted = false;
                Debug.Log("关卡失败：时间耗尽！");
                UIManager.Instance.OnCloseResourcePanel();
            }

            //UpdateTimeText();
            UIManager.Instance.OnUpdateTimeLeftPanel(remainingTime);
        }
        

        public void CheckVictoryCondition()
        {
            if (isEnterEnd && remainingTime > 0f)
            {
                isLevelStarted = false;
                Debug.Log("关卡成功！");
                UIManager.Instance.OpenEndLevelPanel();
                UIManager.Instance.OnCloseTimeLeftPanel();
                SpawnManager.Instance.isLevelStarted = false;
                UIManager.Instance.OnCloseResourcePanel();
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
            regainTimer = 0f;
            remainingTime = levelTimeLimit;
            regainTimeScale = 1f;
            isEnterEnd = false;
            isLevelStarted = false;
        }
    }
}
