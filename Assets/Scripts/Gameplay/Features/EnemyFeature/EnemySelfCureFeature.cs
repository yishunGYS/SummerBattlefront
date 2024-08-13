using System;
using Gameplay.Enemy;
using Sirenix.OdinInspector;
using Systems.Buff;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Features.EnemyFeature
{
    public enum CureConditionType
    {
        HealthBelowPercentage,
        HitIntervalExceeds
    }

    [RequireComponent(typeof(EnemyAgent))]
    public class EnemySelfCureFeature : MonoBehaviour
    {
        private EnemyAgent agent;

        //持续恢复条件
        public CureConditionType conditionType;

        [ShowIf("conditionType", CureConditionType.HealthBelowPercentage)]
        public float healthPercentage;

        [ShowIf("conditionType", CureConditionType.HitIntervalExceeds)]
        public float hitInterval;

        //恢复公式
        [EnumToggleButtons] public SoliderCureFeature.HealingFormula healingFormula;

        [ShowIf("healingFormula", SoliderCureFeature.HealingFormula.BaseAttack)]
        public float baseAttackPercentage;

        [ShowIf("healingFormula", SoliderCureFeature.HealingFormula.BaseMaxHp)]
        public float baseMaxHpPercentage;

        //应用的Buff
        private BuffModel cureBuff;

        private float lastHitTime;
        

        public void OnInit()
        {
            agent = GetComponent<EnemyAgent>();
            AddInBuffManager();
        }

        private void SelfCure()
        {
            if (conditionType == CureConditionType.HealthBelowPercentage)
            {
                if ((float)agent.curHp / agent.GetMaxHp() >= healthPercentage)
                {
                    return;
                }
            }

            if (conditionType == CureConditionType.HitIntervalExceeds)
            {
                if (lastHitTime + hitInterval <= Time.time)
                {
                    return;
                }
            }

            var newHp = agent.curHp + CalculateCureAmount();
            agent.curHp = Math.Clamp(newHp, 0, agent.enemyModel.maxHp);
            Debug.Log("持续治疗"+agent.name+" "+CalculateCureAmount());
        }


        private int CalculateCureAmount()
        {
            if (healingFormula == SoliderCureFeature.HealingFormula.BaseAttack)
            {
                var curAttackData = agent.buffManager.CalculateAttack(agent);
                var attackPoint = curAttackData.attackPoint + curAttackData.magicAttackPoint;
                return Mathf.RoundToInt(attackPoint * baseAttackPercentage);
            }

            if (healingFormula == SoliderCureFeature.HealingFormula.BaseMaxHp)
            {
                return Mathf.RoundToInt(agent.enemyModel.maxHp * baseMaxHpPercentage);
            }

            return 0;
        }

        private void AddInBuffManager()
        {
            cureBuff = new BuffModel
            {
                buff触发类型 = BuffType.持续每秒生效,
                durationTime = 100000000f,
                tickTime = 3f,
            };
            cureBuff.在Tick触发时.AddListener(SelfCure);
            agent.buffManager.AddBuff(cureBuff);
        }
    }
}