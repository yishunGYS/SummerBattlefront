using Managers;

namespace Gameplay.Player.Solider.Attacker.MeleeGodling
{
    public class MeleeGodingLogic : AttackerSoliderLogic
    {
        public MeleeGodingLogic(SoliderAgent agent) : base(agent)
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
