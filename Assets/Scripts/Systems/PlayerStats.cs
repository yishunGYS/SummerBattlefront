using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public class PlayerStats : MonoBehaviour
    {
        public static int Money;
        public int startMoney = 400;

        public static int Lives;
        public int startLives = 20;

        public static int Rounds;

        [Header("��ǰ�ظ�����")]
        public int currentRegainRate;

        [Header("��ǰ������")]
        public int currentLimit;

        [Header("�����೤ʱ���л�����һ���׶�")]
        public int switchPhase;

        [Header("�ظ��ٶ�")]
        public List<int> regainPhase;

        [Header("��Դ����")]
        public List<int> limitPhase;

        private float switchTimer;
        private float regainTimer;
        private int currentPhaseIndex;

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
                Debug.LogError("û�����ûظ��ٶȺ�����");
            }

            Rounds = 0;
            switchTimer = 0f;
            regainTimer = 0f;
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
