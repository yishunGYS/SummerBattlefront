using Gameplay.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Item
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager instance;

        // 存储所有获取的道具
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
            // 将道具的效果存储到items列表中
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
                Debug.LogError("SoliderContainer未找到！");
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
            // 遍历所有存储的道具效果
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
                        Debug.Log($"提升了 {model.soliderName} 的攻击力: +{item.RiseAmount}");
                        Debug.Log($"{model.soliderName} 现在的攻击力: +{model.attackPoint}");
                        break;
                    case RiseStats.Defence:
                        model.defendReducePercent += item.RiseAmount;
                        Debug.Log($"提升了 {model.soliderName} 的防御力减少百分比: +{item.RiseAmount * 100}%");
                        break;
                    case RiseStats.AttackSpeed:
                        model.attackInterval -= item.RiseAmount;
                        if (model.attackInterval < 0.1f)  // 确保攻击速度不会过快
                            model.attackInterval = 0.1f;
                        Debug.Log($"提升了 {model.soliderName} 的攻击速度: 攻击间隔 -{item.RiseAmount} 秒");
                        break;
                }
            }
        }
    }
}
