using System;
using Managers;
using UnityEngine;

namespace Gameplay.Player.Solider.MeleeGodling
{
    public class MeleeGodingLogic : SoliderLogicBase
    {
        public MeleeGodingLogic(SoliderAgent agent) : base(agent)
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
            playerBuffManager.AddBuff(BuffInventoryManager.Instance.GetBuffById(0));  //无敌buff
            MeleeAttack();
        }
    }
}
