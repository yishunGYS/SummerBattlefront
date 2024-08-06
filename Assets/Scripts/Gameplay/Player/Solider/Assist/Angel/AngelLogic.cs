using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player.Solider.Assist.Angel
{
    public class AngelLogic : AssistSoliderLogic
    {
        public AngelLogic(SoliderAgent agent) : base(agent)
        {
        }


        //获取攻击范围内血量/血量百分比最低的友方
        public override void GetTarget()
        {
            base.GetTarget();
            GetTargetBaseMinHp();
        }


        public void GetTargetBaseMinHp()
        {
            List<CureSoliderTarget> tempCureTargets = new List<CureSoliderTarget>();
            
            Collider[] hitColliders =
                Physics.OverlapSphere(soliderAgent.transform.position, soliderModel.attackRange,
                    LayerMask.GetMask("Solider"));

            foreach (var collider in hitColliders)
            {
                var temp = collider.GetComponent<SoliderAgent>();
                var tempTargetHp = temp.soliderLogic.curHp;

                var temTarget = new CureSoliderTarget(tempTargetHp, temp);
                tempCureTargets.Add(temTarget);
                
            }

            SortCureTargetsByMinHp(tempCureTargets);
            for (int i = 0; i < tempCureTargets.Count; i++)
            {
                attackTargets.Add(tempCureTargets[i].target);
            }
            
        }

        private void SortCureTargetsByMinHp(List<CureSoliderTarget> cureTargets)
        {
            cureTargets.Sort((a,b)=>a.hp.CompareTo(b.hp));
        }
    }
}