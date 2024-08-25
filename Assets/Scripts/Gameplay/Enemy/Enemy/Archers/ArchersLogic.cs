namespace Gameplay.Enemy.Enemy.Archers
{
    public class ArchersLogic : AttackEnemyLogic
    {
        public ArchersLogic(EnemyAgent agent) : base(agent)
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
            NormalAttack();
        }
    }
}


