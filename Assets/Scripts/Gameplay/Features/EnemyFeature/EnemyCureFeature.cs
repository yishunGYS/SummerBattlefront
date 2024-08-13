using System;
using Gameplay.Enemy;
using Gameplay.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Features.EnemyFeature
{
    [RequireComponent(typeof(EnemyAgent))]
    public class EnemyCureFeature : MonoBehaviour
    {
        [EnumToggleButtons] public SoliderCureFeature.HealingFormula healingFormula;


        [ShowIf("healingFormula", SoliderCureFeature.HealingFormula.BaseAttack)]
        public float baseAttackPercentage;

        [ShowIf("healingFormula", SoliderCureFeature.HealingFormula.BaseMaxHp)]
        public float baseMaxHpPercentage;

        private EnemyAgent agent;

        private void Awake()
        {
            agent = GetComponent<EnemyAgent>();
        }


        //单次治疗
        public void Cure()
        {
            foreach (var enemy in agent.enemyLogic.attackTargets)
            {
                EnemyAgent targetAgent = enemy as EnemyAgent;
                if (targetAgent != null)
                {
                    if (targetAgent.curHp < targetAgent.enemyModel.maxHp)
                    {
                        int newHp = targetAgent.curHp + CalculateCureAmount();
                        targetAgent.curHp = Math.Clamp(newHp, 0, targetAgent.enemyModel.maxHp);
                        Debug.Log("治疗"+targetAgent.name+" "+CalculateCureAmount());
                    }
                }
            }
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
    }
}
