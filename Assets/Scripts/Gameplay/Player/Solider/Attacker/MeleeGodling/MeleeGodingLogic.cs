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
            DistanceBasedGetTarget();
        }

 

        public override void Attack()
        {
            base.Attack();
            MeleeAttack();
            
            
        }


    }
}
