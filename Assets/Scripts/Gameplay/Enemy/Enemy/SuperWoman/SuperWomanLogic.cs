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
            GetFocusTarget();
        }

        public override void Attack()
        {
            base.Attack();
            FocusAttack();
        }
    }
}


