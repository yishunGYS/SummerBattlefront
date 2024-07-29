using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Player.Solider
{
    public class RangeGodlingLogic: SoliderLogicBase
    {
        private RangeGodling rangeGodlingAgent;
        public RangeGodlingLogic(SoliderAgent agent) : base(agent)
        {
            rangeGodlingAgent = (RangeGodling)agent;
        }

        public override void GetTarget()
        {
            base.GetTarget();
            MultiAttackSoliderGetTarget();
        }

        // public override void Attack()
        // {
        //     base.Attack();
        //     if (isAttackReady)
        //     {
        //         //发射投掷物
        //         foreach (var target in soliderAgent.soliderLogic.attackTargets)
        //         {
        //             var go =  GameObject.Instantiate(rangeGodlingAgent.projectile, rangeGodlingAgent.transform.position, Quaternion.identity);
        //             go.OnInit(target.transform.position,rangeGodlingAgent);
        //         }
        //     }
        //
        //     CalculateCd();
        // }

        public override void Attack()
        {
            base.Attack();
            if (isAttackReady)
            {
                CalculateCd();
                //发射投掷物
                for (int i = soliderAgent.soliderLogic.attackTargets.Count-1; i >= 0; i--)
                {
                    Debug.Log("攻击！！！");
                    var go =  GameObject.Instantiate(rangeGodlingAgent.projectile, rangeGodlingAgent.transform.position, Quaternion.identity);
                    go.OnInit(soliderAgent.soliderLogic.attackTargets[i].transform.position,rangeGodlingAgent);
                }
            }
            
        }
    }
}