using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player.Solider.Valkyrie
{
    public class ValkyrieLogic : SoliderLogicBase
    {
        public ValkyrieLogic(SoliderAgent agent) : base(agent)
        {
        }

        public override void GetTarget()
        {
            base.GetTarget();
            DistanceBasedGetTarget();
        }

        public override void Attack()
        {
            base.Attack();
            MeleeAOE();
        }

        private void MeleeAOE()
        {
            if (isAttackReady)
            {
                CalculateCd();
                for (int i = soliderAgent.soliderLogic.attackTargets.Count - 1; i >= 0; i--)
                {
                    Debug.Log("µ¥ÌåAOE¹¥»÷£¡£¡£¡");
                    soliderAgent.soliderLogic.attackTargets[i].enemyLogic.OnTakeAOEDamage(
                        soliderAgent.soliderModel.attackPoint,
                        soliderAgent.soliderModel.magicAttackPoint, 
                        soliderAgent,
                        soliderAgent.soliderModel.attackAoeRange);
                }

            }
        }
    }

}