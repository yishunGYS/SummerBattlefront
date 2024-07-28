using UnityEngine;

namespace Gameplay.Enemy.Enemy.SmallGuaiGuai
{
    public class SmallGuaiGuaiLogic : EnemyLogicBase
    {


        public SmallGuaiGuaiLogic(EnemyAgent agent) : base(agent)
        {
        }

        public override void GetTarget()
        {
            base.GetTarget();
            SingleAttackEnemyGetTarget();
        }

        public override void Attack()
        {
            base.Attack();
            MeleeAttack();
        }
    }
}
