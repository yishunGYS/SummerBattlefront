using System.Collections.Generic;
using System.Linq;
using Gameplay.Enemy;
using UnityEngine;

namespace Gameplay.Player.Solider.Attacker
{
    public class AttackerSoliderLogic : SoliderLogicBase
    {
        protected AttackerSoliderLogic(SoliderAgent agent) : base(agent)
        {
        }

        #region 获取目标

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
                if (temp == null)
                {
                    Debug.Log("temp is null");
                    continue;
                }
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
                if (i> tempAttackTargets.Count - 1)
                {
                    break;
                }
                attackTargets.Add(tempAttackTargets[i].target);
            }
        }

        private void SortMultiTargetsByDistance(List<AttackEnemyTarget> attackTargets)
        {
            attackTargets.Sort((a, b) => a.dis.CompareTo(b.dis));
        }

        //用于判别地面不能打敌人Tower
        public override bool CheckMatchAttackType(EnemyAgent target)
        {
            //todo 若attackEnemyType是多种，那么----待扩展
            if ((soliderModel.attackEnemyType & target.enemyModel.enemyType) == UnitType.None)
            {
                return false;
            }

            return true;
        }

        protected override bool HasAttackTarget()
        {
            base.HasAttackTarget();
            Collider[] hitColliders =
                Physics.OverlapSphere(soliderAgent.transform.position, soliderModel.attackRange,
                    LayerMask.GetMask("Enemy"));
            var count = hitColliders.Length;
            foreach (var collider in hitColliders)
            {
                var temp = collider.GetComponent<EnemyAgent>();
                if (temp == null)
                {
                    continue;
                }
                if (!CheckMatchAttackType(temp))
                {
                    count -= 1;
                }
            }
            
            if (count <= 0)
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

        #endregion


        #region 攻击

        //最基础的近战
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
                        Debug.Log("攻击！！！");
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
                    Debug.Log("单体AOE攻击！！！");
                    if (enemyAgent != null)
                    {
                        enemyAgent.enemyLogic.OnTakeAOEDamage(
                            soliderAgent,
                            soliderModel.attackAoeRange);
                    }
                }
            }
        }

        #endregion
    }
}