using UnityEngine;

namespace Gameplay.Player.Solider.Attacker.RangeGodling
{
    public class RangeGodlingLogic: AttackerSoliderLogic
    {
        private Solider.RangeGodling rangeGodlingAgent;
        public RangeGodlingLogic(SoliderAgent agent) : base(agent)
        {
            rangeGodlingAgent = (Solider.RangeGodling)agent;
        }

        public override void GetTarget()
        {
            base.GetTarget();
            DistanceBasedGetTarget();
        }
        

        public override void Attack()
        {
            base.Attack();
            if (isAttackReady)
            {
                CalculateCd();
                //发射投掷物
                for (int i = attackTargets.Count-1; i >= 0; i--)
                {
                    Debug.Log("攻击！！！");
                    var go =  GameObject.Instantiate(rangeGodlingAgent.projectile, rangeGodlingAgent.transform.position, Quaternion.identity);
                    go.OnInit(rangeGodlingAgent,attackTargets[i]);
                }
            }
        }
    }
}