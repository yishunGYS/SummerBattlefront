using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Enemy.Enemy.SuperWoman
{
    public class SuperWomanLogic : AttackEnemyLogic
    {
        public SuperWomanLogic(EnemyAgent agent) : base(agent)
        {
        }

        public override void GetTarget()
        {
            base.GetTarget();
            enemyGetTargetFeature.GetTarget();
        }

        public override void Attack()
        {
            base.Attack();
            MeleeAOE();
        }
    }
}


