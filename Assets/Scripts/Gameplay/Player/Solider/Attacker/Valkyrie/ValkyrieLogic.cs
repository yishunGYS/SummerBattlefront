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
            DistanceBasedGetTarget();
        }

        public override void Attack()
        {
            base.Attack();
            MeleeAOE();
        }



        


    }

}