using UnityEngine;

namespace Gameplay.Player.Solider.MeleeGodling
{
    public class MeleeGodingLogic : SoliderLogicBase
    {
        public MeleeGodingLogic(SoliderAgent agent) : base(agent)
        {
        }

        public override void GetTarget()
        {
            base.GetTarget();
            SingleAttackSoliderGetTarget();
        }

        public override void Attack()
        {
            base.Attack();
            MeleeAttack();
        }
    }
}
