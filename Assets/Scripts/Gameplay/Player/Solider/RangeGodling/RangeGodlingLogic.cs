using UnityEngine;

namespace Gameplay.Player.Solider
{
    public class RangeGodlingLogic: SoliderLogicBase
    {
        private RangeGodling rangeGodlingAgent;
        public RangeGodlingLogic(SoliderAgent agent) : base(agent)
        {
            rangeGodlingAgent = (RangeGodling)agent;
        }

        public override void GetTarget()
        {
            base.GetTarget();
            MultiAttackSoliderGetTarget();
        }

        public override void Attack()
        {
            base.Attack();
            if (isAttackReady)
            {
                //发射投掷物
                foreach (var target in soliderAgent.soliderLogic.attackTargets)
                {
                    var go =  GameObject.Instantiate(rangeGodlingAgent.projectile, rangeGodlingAgent.transform.position, Quaternion.identity);
                    go.OnInit(target.transform.position,rangeGodlingAgent);
                }
            }
        }
    }
}