namespace Gameplay.Enemy.Enemy.SnowWoman
{
    public class SnowWomanLogic : EnemyLogicBase
    {
        public SnowWomanLogic(EnemyAgent agent) : base(agent)
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
            MeleeAOE();
        }
    }
}


