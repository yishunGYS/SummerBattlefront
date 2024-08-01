using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player.Solider.SkeletonArmy
{
    public class SkeletonArmyLogic : SoliderLogicBase
    {
        public SkeletonArmyLogic(SoliderAgent agent) : base(agent)
        {
        }

        public override void GetTarget()
        {
            base.GetTarget();
            SingleAttackSoliderGetTarget();
        }

        public override void Attack()
        {
            base.Attack();
            MeleeAttack();
        }
    }

}
