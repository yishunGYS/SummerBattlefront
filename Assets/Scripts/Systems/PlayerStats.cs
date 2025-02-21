using System;
using Managers;
using Sirenix.OdinInspector;
using Systems.Edu;
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
        [HideInInspector]public int startMoney;

        [Header("当前回复速率")]
        [HideInInspector]public float currentRegainRate;

        [Header("当前的上限")] 
        [HideInInspector]public int currentLimit;

        //[Header("关卡时间限制（秒）")]
        //public float levelTimeLimit = 300f;

        [Header("关卡资源上限")]
        public int levelResourceLimit;
        
        //public float remainingTime;
        public float remainingResource;
        
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
                //Test
                levelResourceLimit =  LevelManager.Instance.GetCurrentLevelResource();
                ResetPlayerStats();
            }
            
        }

        void FixedUpdate()
        {
            if (isLevelStarted)
            {
                RegainMoneyOverTime();
                UIManager.Instance.OnUpdateResourceLeftPanel(remainingResource+Money);
                UIManager.Instance.OnUpdateResourcePanel();
            }
        }

        public void StartLevel()
        {
            if (!isLevelStarted)
            {
                isLevelStarted = true;
                UIManager.Instance.OnOpenResourcePanel();
                Debug.Log("关卡开始！");
            }
            
            if (FindObjectOfType<EduSystem>())
            {
                UIManager.Instance.OnShowEduPanel();
            }
        }

        private void CalculateMoneyAndRemainingResource()
        {
            Money = levelResourceLimit<=10 ? levelResourceLimit : 10;
            remainingResource = levelResourceLimit - Money;
        }

        void RegainMoneyOverTime()
        {
            if (remainingResource<=0)
            {
                return;
            }
            if (Money >= currentLimit)
            {
                return;
            }
            
            regainTimer += Time.fixedDeltaTime * regainTimeScale;

            if (regainTimer >= 1f)
            {
                Money += currentRegainRate;
                regainTimer = 0f;

                if (Money > currentLimit)
                {
                    Money = currentLimit;
                }

                remainingResource -= currentRegainRate;
            }

            if (Money>=currentLimit)
            {
                UIManager.Instance.OnReachMax();
            }
            
            
        }
        

        public void CheckVictoryCondition()
        {
            if (isEnterEnd)
            {
                isLevelStarted = false;
                Debug.Log("关卡成功！");
                UIManager.Instance.OpenEndLevelPanel();
                SpawnManager.Instance.isLevelStarted = false;
                UIManager.Instance.OnCloseTimeLeftPanel();
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
            regainTimer = 0f;
            //remainingResource = levelResourceLimit;
            regainTimeScale = 1f;
            isEnterEnd = false;
            isLevelStarted = false;
            CalculateMoneyAndRemainingResource();
            SpawnManager.Instance.selectedCharacter = null;
        }
    }
}
