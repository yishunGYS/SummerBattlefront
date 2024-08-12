using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player.Solider.Assist
{
    class CureSoliderTarget
    {
        public int hp;
        public SoliderAgent target;

        public CureSoliderTarget(int hp, SoliderAgent target)
        {
            this.hp = hp;
            this.target = target;
        }
    }

    public class AssistSoliderLogic : SoliderLogicBase
    {
        protected AssistSoliderLogic(SoliderAgent agent) : base(agent)
        {
        }

        protected override bool HasAttackTarget()
        {
            Collider[] hitColliders =
                Physics.OverlapSphere(soliderAgent.transform.position, soliderModel.attackRange,
                    LayerMask.GetMask("Solider"));

            int colliderCount = hitColliders.Length;
            foreach (var collider in hitColliders)
            {
                if (collider == soliderAgent.GetComponent<Collider>())
                {
                    colliderCount--;
                }
            }
            if (colliderCount<1)
            {
                return false;
            }

            return true;
        }
        
        
        //获取攻击范围内血量/血量百分比最低的友方
        protected void GetTargetBaseMinHp()
        {
            List<CureSoliderTarget> tempCureTargets = new List<CureSoliderTarget>();

            Collider[] hitColliders =
                Physics.OverlapSphere(soliderAgent.transform.position, soliderModel.attackRange,
                    LayerMask.GetMask("Solider"));

            foreach (var collider in hitColliders)
            {
                var temp = collider.GetComponent<SoliderAgent>();
                var tempTargetHp = temp.curHp;
                if (tempTargetHp == temp.soliderModel.maxHp)
                {
                    continue;
                }

                var temTarget = new CureSoliderTarget(tempTargetHp, temp);
                tempCureTargets.Add(temTarget);
            }

            SortCureTargetsByMinHp(tempCureTargets);
            for (int i = 0; i < soliderAgent.soliderModel.attackNum; i++)
            {
                if (tempCureTargets.Count <= i)
                {
                    return;
                }

                attackTargets.Add(tempCureTargets[i].target);
            }
        }

        private void SortCureTargetsByMinHp(List<CureSoliderTarget> cureTargets)
        {
            cureTargets.Sort((a, b) => a.hp.CompareTo(b.hp));
        }


        //其实士兵获取目标前都会clear掉attackTargets
        public override void RemoveTarget(UnitAgent target)
        {
            base.RemoveTarget(target);
            SoliderAgent agent = target as SoliderAgent;
            if (attackTargets.Contains(agent) && agent!= null)
            {
                attackTargets.Remove(agent);
                Debug.Log($"Target removed: {agent.soliderModel.soliderName}");
            }
        }
    }
}