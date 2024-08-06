using System;
using Gameplay.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Features
{
    [RequireComponent(typeof(SoliderAgent))]
    public class CureFeature : MonoBehaviour
    {
        public enum HealingFormula
        {
            BaseAttack,
            BaseMaxHp,
        }

        [EnumToggleButtons] public HealingFormula healingFormula;


        [ShowIf("healingFormula", HealingFormula.BaseAttack)]
        public float baseAttackPercentage;

        [ShowIf("healingFormula", HealingFormula.BaseMaxHp)]
        public float baseMaxHpPercentage;

        private SoliderAgent agent;

        private void Awake()
        {
            agent = GetComponent<SoliderAgent>();
        }


        public void Cure()
        {
            foreach (var solider in agent.soliderLogic.attackTargets)
            {
                SoliderAgent targetAgent = solider as SoliderAgent;
                if (targetAgent != null)
                {
                    if (targetAgent.soliderLogic.curHp < targetAgent.soliderModel.maxHp)
                    {
                        int newHp = targetAgent.soliderLogic.curHp + CalculateCureAmount();
                        targetAgent.soliderLogic.curHp = Math.Clamp(newHp, 0, targetAgent.soliderModel.maxHp);
                    }
                }
            }
        }

        private int CalculateCureAmount()
        {
            if (healingFormula == HealingFormula.BaseAttack)
            {
                var curAttackData = agent.soliderLogic.playerBuffManager.CalculateAttack(agent);
                var attackPoint = curAttackData.attackPoint + curAttackData.magicAttackPoint;
                return Mathf.RoundToInt(attackPoint * baseAttackPercentage);
            }

            if (healingFormula == HealingFormula.BaseMaxHp)
            {
                return Mathf.RoundToInt(agent.soliderModel.maxHp * baseAttackPercentage);
            }

            return 0;
        }
    }
}