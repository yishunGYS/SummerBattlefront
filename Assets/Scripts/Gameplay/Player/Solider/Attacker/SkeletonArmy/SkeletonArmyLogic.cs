namespace Gameplay.Player.Solider.Attacker.SkeletonArmy
{
    public class SkeletonArmyLogic : AttackerSoliderLogic
    {
        public SkeletonArmyLogic(SoliderAgent agent) : base(agent)
        {
        }

        public override void GetTarget()
        {
            base.GetTarget();
            if (attackTargets.Contains(blocker))
            {
                return;
            }
            DistanceBasedGetTarget();
        }

        public override void Attack()
        {
            base.Attack();
            MeleeAttack();
        }
    }

}
