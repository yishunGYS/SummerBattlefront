using UnityEngine;

namespace Gameplay.Player.Solider.Attacker.Valkyrie
{
    public class ValkyrieLogic : AttackerSoliderLogic
    {
        public ValkyrieLogic(SoliderAgent agent) : base(agent)
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
            MeleeAOE();
        }



        


    }

}