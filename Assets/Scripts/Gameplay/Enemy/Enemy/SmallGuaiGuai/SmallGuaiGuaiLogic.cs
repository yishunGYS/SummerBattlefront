using UnityEngine;

namespace Gameplay.Enemy.Enemy.SmallGuaiGuai
{
    public class SmallGuaiGuaiLogic : AttackEnemyLogic
    {


        public SmallGuaiGuaiLogic(EnemyAgent agent) : base(agent)
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
