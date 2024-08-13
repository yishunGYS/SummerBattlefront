using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Enemy.Enemy
{
    public class AttackEnemyLogic : EnemyLogicBase
    {
        protected AttackEnemyLogic(EnemyAgent agent) : base(agent)
        {
        }

        #region ��ȡĿ��
        
        protected void DistanceBasedEnemyGetTarget()
        {
            List<AttackSoliderTarget> tempDistanceTargets = new List<AttackSoliderTarget>();
        
            Collider[] hitColliders =
                Physics.OverlapSphere(enemyAgent.transform.position, enemyModel.attackRange,
                    LayerMask.GetMask("Solider"));
            foreach (var collider in hitColliders)
            {
                var tempDis = Vector3.Distance(enemyAgent.transform.position, collider.transform.position);
                var temp = collider.GetComponent<SoliderAgent>();
                if (!CheckMatchAttackType(temp))
                {
                    continue;
                }
        
                var tempTarget = new AttackSoliderTarget(tempDis, temp);
                tempDistanceTargets.Add(tempTarget);
            }
        
            SortMultiTargetsByDistance(tempDistanceTargets);
            for (int i = 0; i < enemyModel.attackNum; i++)
            {
                attackTargets.Add(tempDistanceTargets[i].target);
            }
        }

        
        //GetFocusTarget�߼��е����⣺��Ŀ��������������һ�����ˣ�ֻ��Ҫ��һ��Ŀ�꣬���߼����������Ŀ�ꡣ
        //todo ����
        protected void GetFocusTarget()
        {
            if (HasFocusTarget())
                return;

            var remainTargetNum = CalculateRemainTargetNum();
            
            var minDis = 10000f;
            SoliderAgent singleTarget = null;
            List<AttackSoliderTarget> tempDistanceTargets = new List<AttackSoliderTarget>();
            Collider[] hitColliders =
                Physics.OverlapSphere(enemyAgent.transform.position, enemyModel.attackRange,
                    LayerMask.GetMask("Solider"));


            foreach (var collider in hitColliders)
            {
                var tempDis = Vector3.Distance(enemyAgent.transform.position, collider.transform.position);
                var temp = collider.GetComponent<SoliderAgent>();
                if (!CheckMatchAttackType(temp))
                {
                    continue;
                }

                if (attackTargets.Contains(temp))
                {
                    continue;
                }

                var tempTarget = new AttackSoliderTarget(tempDis, temp);
                tempDistanceTargets.Add(tempTarget);
            }

            SortMultiTargetsByDistance(tempDistanceTargets);
            for (int i = 0; i < remainTargetNum; i++)
            {
                attackTargets.Add(tempDistanceTargets[i].target);
            }
        }

        private void SortMultiTargetsByDistance(List<AttackSoliderTarget> distanceTargets)
        {
            distanceTargets.Sort((a, b) => a.dis.CompareTo(b.dis));
        }
        
        private bool HasFocusTarget()
        {
            if (attackTargets.Count >= enemyModel.attackNum)
                return true;
            return false;
        }

        private int CalculateRemainTargetNum()
        {
            return enemyModel.attackNum - attackTargets.Count;
        }

        private bool CheckMatchAttackType(SoliderAgent target)
        {
            if ((enemyModel.attackSoliderType & target.soliderModel.soliderType) == UnitType.None)
            {
                return false;
            }
            return true;
        }


        protected override bool HasAttackTarget()
        {
            base.HasAttackTarget();
            Collider[] hitColliders =
                Physics.OverlapSphere(enemyAgent.transform.position, enemyModel.attackRange,
                    LayerMask.GetMask("Solider"));

            int soliderCount = 0;
            foreach (var hitCollider in hitColliders)
            {
                // ���˵��Լ�����ײ��
                if (hitCollider.gameObject != enemyAgent.gameObject)
                {
                    soliderCount++;
                    // ���������ʵ�ֹ����߼�
                }
            }

            if (soliderCount <= 0)
            {
                return false;
            }

            return true;
        }


        public override void RemoveAttackTarget(UnitAgent target)
        {
            SoliderAgent soliderAgent = target as SoliderAgent;
            if (attackTargets.Contains(soliderAgent) && soliderAgent != null)
            {
                attackTargets.Remove(soliderAgent);
                Debug.Log($"Target removed: {soliderAgent.soliderModel.soliderName}");
            }
            else
            {
                Debug.Log("Target not found in the list.");
            }
        }

        #endregion

        #region ����
        
        //��������
        protected void FocusAttack()
        {
            if (isAttackReady)
            {
                CalculateCd();
                for (int i = attackTargets.Count - 1; i >= 0; i--)
                {
                    SoliderAgent soliderAgent = attackTargets[i] as SoliderAgent;
                    if (soliderAgent!= null)
                    {
                        soliderAgent.soliderLogic.OnTakeDamage(enemyAgent);
                        Debug.Log("����רע����");
                    }
                }
            }
        }

        //רעAOE����
        protected void MeleeAOE()
        {
            if (isAttackReady)
            {
                CalculateCd();
                for (int i = attackTargets.Count - 1; i >= 0; i--)
                {
                    SoliderAgent soliderAgent = attackTargets[i] as SoliderAgent;
                    if (soliderAgent!= null)
                    {
                        Debug.Log("����AOE����������");
                        soliderAgent.soliderLogic.OnTakeAOEDamage(
                            enemyAgent,
                            enemyAgent.enemyModel.attackAoeRange);
                    }

                }
            }
        }

        #endregion
    }
}
