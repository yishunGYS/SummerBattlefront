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
            GetFocusTarget();
        }

        public override void Attack()
        {
            base.Attack();
            FocusAttack();
        }
    }
}


