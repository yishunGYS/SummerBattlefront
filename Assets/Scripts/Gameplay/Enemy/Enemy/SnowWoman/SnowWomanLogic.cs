namespace Gameplay.Enemy.Enemy.SnowWoman
{
    public class SnowWomanLogic : AttackEnemyLogic
    {
        public SnowWomanLogic(EnemyAgent agent) : base(agent)
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


