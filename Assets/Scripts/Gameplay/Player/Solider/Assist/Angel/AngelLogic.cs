using System.Collections.Generic;
using Gameplay.Features;
using UnityEngine;

namespace Gameplay.Player.Solider.Assist.Angel
{
    //�չ�ΪѪ����͵��ѷ���Ѫ
    public class AngelLogic : AssistSoliderLogic
    {
        public AngelLogic(SoliderAgent agent) : base(agent)
        {
        }


        //��ȡ������Χ��Ѫ��/Ѫ���ٷֱ���͵��ѷ�
        public override void GetTarget()
        {
            base.GetTarget();
            GetTargetBaseMinHp();
        }


        public override bool HasAttackTarget()
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

        private void GetTargetBaseMinHp()
        {
            List<CureSoliderTarget> tempCureTargets = new List<CureSoliderTarget>();
            
            Collider[] hitColliders =
                Physics.OverlapSphere(soliderAgent.transform.position, soliderModel.attackRange,
                    LayerMask.GetMask("Solider"));

            foreach (var collider in hitColliders)
            {
                var temp = collider.GetComponent<SoliderAgent>();
                var tempTargetHp = temp.soliderLogic.curHp;
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
            cureTargets.Sort((a,b)=>a.hp.CompareTo(b.hp));
        }

        public override void Attack()
        {
            base.Attack();
            if (isAttackReady)
            {
                CalculateCd();
                soliderAgent.GetComponent<CureFeature>().Cure();
            }
            
        }
    }
}