using Gameplay.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Item
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager instance;

        // �洢���л�ȡ�ĵ���
        public List<RiseSoliderStatsItem> items = new List<RiseSoliderStatsItem>();

        private GameObject SoliderContainer;

        private void Awake()
        {
            if (instance != null)
            {
                return;
            }
            instance = this;
        }

        public void OnStart()
        {
            SoliderContainer = GameObject.Find("SoliderContainer");
        }

        public void AddItem(RiseSoliderStatsItem item)
        {
            // �����ߵ�Ч���洢��items�б���
            items.Add(new RiseSoliderStatsItem
            {
                RiseStats = item.RiseStats,
                RiseAmount = item.RiseAmount
            });
        }

        public void UseSoliderItem(RiseSoliderStatsItem item)
        {
            if (SoliderContainer == null)
            {
                Debug.LogError("SoliderContainerδ�ҵ���");
                return;
            }

            foreach (Transform child in SoliderContainer.transform)
            {
                SoliderAgent soliderAgent = child.GetComponent<SoliderAgent>();
                if (soliderAgent != null)
                {
                    ApplyItemEffect(soliderAgent, item);
                }
            }
        }

        public void RiseSoliderStats(SoliderAgent soliderAgent)
        {
            // �������д洢�ĵ���Ч��
            foreach (RiseSoliderStatsItem item in items)
            {
                ApplyItemEffect(soliderAgent, item);
            }
        }

        private void ApplyItemEffect(SoliderAgent soliderAgent, RiseSoliderStatsItem item)
        {
            SoliderModelBase model = soliderAgent.soliderModel;

            if (model != null)
            {
                switch (item.RiseStats)
                {
                    case RiseStats.Attack:
                        model.attackPoint += (int)item.RiseAmount;
                        Debug.Log($"������ {model.soliderName} �Ĺ�����: +{item.RiseAmount}");
                        Debug.Log($"{model.soliderName} ���ڵĹ�����: +{model.attackPoint}");
                        break;
                    case RiseStats.Defence:
                        model.defendReducePercent += item.RiseAmount;
                        Debug.Log($"������ {model.soliderName} �ķ��������ٰٷֱ�: +{item.RiseAmount * 100}%");
                        break;
                    case RiseStats.AttackSpeed:
                        model.attackInterval -= item.RiseAmount;
                        if (model.attackInterval < 0.1f)  // ȷ�������ٶȲ������
                            model.attackInterval = 0.1f;
                        Debug.Log($"������ {model.soliderName} �Ĺ����ٶ�: ������� -{item.RiseAmount} ��");
                        break;
                }
            }
        }
    }
}
