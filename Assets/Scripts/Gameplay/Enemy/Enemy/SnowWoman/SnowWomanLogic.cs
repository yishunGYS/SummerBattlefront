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
            GetFocusTarget();
        }

        public override void Attack()
        {
            base.Attack();
            MeleeAOE();
        }
    }
}


