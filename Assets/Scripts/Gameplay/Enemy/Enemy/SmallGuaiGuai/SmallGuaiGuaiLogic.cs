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
            enemyGetTargetFeature.GetTarget();
            Debug.Log("GetTarget");
        }

        public override void Attack()
        {
            base.Attack();
            NormalAttack();
        }
    }
}
