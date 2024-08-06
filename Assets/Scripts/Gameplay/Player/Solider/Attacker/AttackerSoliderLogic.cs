using System.Collections.Generic;
using System.Linq;
using Gameplay.Enemy;
using UnityEngine;

namespace Gameplay.Player.Solider.Attacker
{
    public class AttackerSoliderLogic : SoliderLogicBase
    {
        //public List<EnemyAgent> attackTargets = new List<EnemyAgent>();


        protected AttackerSoliderLogic(SoliderAgent agent) : base(agent)
        {
        }

        protected void DistanceBasedGetTarget()
        {
            List<AttackEnemyTarget> tempAttackTargets = new List<AttackEnemyTarget>();

            Collider[] hitColliders =
                Physics.OverlapSphere(soliderAgent.transform.position, soliderModel.attackRange,
                    LayerMask.GetMask("Enemy"));

            foreach (var collider in hitColliders)
            {
                var tempDis = Vector3.Distance(soliderAgent.transform.position, collider.transform.position);
                var temp = collider.GetComponent<EnemyAgent>();
                if (!CheckMatchAttackType(temp))
                {
                    continue;
                }

                var tempTarget = new AttackEnemyTarget(tempDis, temp);
                tempAttackTargets.Add(tempTarget);
            }

            SortMultiTargetsByDistance(tempAttackTargets);
            for (int i = 0; i < soliderModel.attackNum; i++)
            {
                attackTargets.Add(tempAttackTargets[i].target);
            }
        }

        private void SortMultiTargetsByDistance(List<AttackEnemyTarget> attackTargets)
        {
            attackTargets.Sort((a, b) => a.dis.CompareTo(b.dis));
        }

        protected override bool CheckMatchAttackType(EnemyAgent target)
        {
            //todo ��attackEnemyType�Ƕ��֣���ô----����չ
            if (target.enemyModel.enemyType != soliderModel.attackEnemyType)
            {
                return false;
            }

            return true;
        }

        public override void RemoveTarget(UnitAgent target)
        {
            EnemyAgent enemyAgent = target as EnemyAgent;
            if (attackTargets.Contains(enemyAgent) && enemyAgent != null)
            {
                attackTargets.Remove(enemyAgent);
                Debug.Log($"Target removed: {enemyAgent.enemyModel.enemyName}");
            }
        }


        protected override void ClearTarget()
        {
            base.ClearTarget();
            attackTargets.Clear();
        }


        #region ����

        //������Ľ�ս
        protected void MeleeAttack()
        {
            if (isAttackReady)
            {
                CalculateCd();
                for (int i = attackTargets.Count - 1; i >= 0; i--)
                {
                    EnemyAgent enemyAgent = attackTargets[i] as EnemyAgent;
                    if (enemyAgent != null)
                    {
                        Debug.Log("����������");
                        enemyAgent.enemyLogic.OnTakeDamage(soliderAgent);
                    }
                }
            }
        }

        protected void MeleeAOE()
        {
            if (isAttackReady)
            {
                CalculateCd();
                for (int i = attackTargets.Count - 1; i >= 0; i--)
                {
                    EnemyAgent enemyAgent = attackTargets[i] as EnemyAgent;
                    Debug.Log("����AOE����������");
                    if (enemyAgent != null)
                    {
                        enemyAgent.enemyLogic.OnTakeAOEDamage(
                            soliderAgent,
                            5);
                    }
                }
            }
        }

        #endregion
    }
}