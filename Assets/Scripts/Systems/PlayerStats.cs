using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

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

        [Header("经过多长时间切换到下一个阶段")]
        public int switchPhase;

        [Header("回复速度")]
        public List<int> regainPhase;

        [Header("资源上限")]
        public List<int> limitPhase;

        private float switchTimer;
        private float regainTimer;
        private int currentPhaseIndex;

        // 定义OnMoneyChanged事件
        public static event Action<int, int> OnMoneyChanged;

        void Awake()
        {
            // 初始化实例
            Instance = this;
        }

        void Start()
        {
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
        }

        void FixedUpdate()
        {
            RegainMoneyOverTime();
            SwitchPhaseOverTime();
        }

        void RegainMoneyOverTime()
        {
            regainTimer += Time.fixedDeltaTime;

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

        void SwitchPhaseOverTime()
        {
            switchTimer += Time.fixedDeltaTime;

            if (switchTimer >= switchPhase)
            {
                currentPhaseIndex++;

                if (currentPhaseIndex < regainPhase.Count && currentPhaseIndex < limitPhase.Count)
                {
                    currentRegainRate = regainPhase[currentPhaseIndex];
                    currentLimit = limitPhase[currentPhaseIndex];
                }

                switchTimer = 0f;
            }
        }
    }
}
