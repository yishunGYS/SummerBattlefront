using System.Collections.Generic;
using Gameplay.Features;
using UnityEngine;

namespace Gameplay.Player.Solider.Assist.Angel
{
    //普攻为血量最低的友方加血
    public class AngelLogic : AssistSoliderLogic
    {
        public AngelLogic(SoliderAgent agent) : base(agent)
        {
        }


        
        public override void GetTarget()
        {
            base.GetTarget();
            GetTargetBaseMinHp();
        }




        

        public override void Attack()
        {
            base.Attack();
            if (isAttackReady)
            {
                CalculateCd();
                soliderAgent.GetComponent<SoliderCureFeature>().Cure();
            }
            
        }
    }
}