using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Enemy.Enemy.SuperWoman
{
    public class SuperWomanLogic : EnemyLogicBase
    {
        public SuperWomanLogic(EnemyAgent agent) : base(agent)
        {
        }

        public override void GetTarget()
        {
            base.GetTarget();
            DistanceBasedEnemyGetTarget();
        }

        public override void Attack()
        {
            base.Attack();
            FocusAttack();
        }
    }
}


