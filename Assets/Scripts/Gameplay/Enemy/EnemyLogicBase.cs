using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Enemy
{
    public class AttackSoliderTarget
    {
        public float dis;
        public SoliderAgent target;

        public AttackSoliderTarget(float dis, SoliderAgent target)
        {
            this.dis = dis;
            this.target = target;
        }
    }

    public class EnemyLogicBase
    {
        private EnemyAgent enemyAgent;
        private EnemyModelBase enemyModel;

        private const float frontCheckDistance = 2f;
        private List<SoliderAgent> attackTargets = new List<SoliderAgent>();
        

        public EnemyLogicBase(EnemyAgent agent)
        {
            enemyAgent = agent;
            enemyModel = enemyAgent.enemyModel;
        }
        
        
        #region 攻击判定

        public bool CheckCanAttack()
        {
            Collider[] hitColliders =
                Physics.OverlapSphere(enemyAgent.transform.position, frontCheckDistance,
                    LayerMask.GetMask("Solider"));

            int soliderCount = 0;
            foreach (var hitCollider in hitColliders)
            {
                // 过滤掉自己的碰撞体
                if (hitCollider.gameObject != enemyAgent.gameObject)
                {
                    soliderCount++;
                    Debug.Log("Soilder detected: " + hitCollider.gameObject.name);
                    // 在这里可以实现攻击逻辑
                }
            }

            if (soliderCount <= 0)
            {
                Debug.Log("附近没士兵");
                return false;
            }

            Debug.Log($"附近有{soliderCount}个士兵");
            return true;
        }

        private void ClearTarget()
        {
            attackTargets.Clear();
        }
        
        //GetTarget需要每帧调吗，会不会很耗性能
        private void GetTarget()
        {
            //每次获取新的目标前，先把已有的目标清除
            ClearTarget();

            Collider[] hitColliders =
                Physics.OverlapSphere(enemyAgent.transform.position, enemyModel.attackRange,
                    LayerMask.GetMask("Solider"));

            //若是单攻
            var minDis = 10000f;
            SoliderAgent singleTarget = null;
            List<SoliderAgent> tempMultiTarget = new List<SoliderAgent>();
            List<AttackSoliderTarget> tempAttackTargets = new List<AttackSoliderTarget>();
            if (enemyModel.attackNum == 1)
            {
                foreach (var collider in hitColliders)
                {
                    var tempDis = Vector3.Distance(enemyAgent.transform.position, collider.transform.position);
                    var temp = collider.GetComponent<SoliderAgent>();
                    if (!CheckMatchAttackType(temp))
                    {
                        continue;
                    }
                    if (tempDis <= minDis)
                    {
                        minDis = tempDis;
                        singleTarget = temp;
                    }
  
                }
                attackTargets.Add(singleTarget);
            }

            //若是多个目标
            else if(enemyModel.attackNum > 1)
            {
                foreach (var collider in hitColliders)
                {
                    var tempDis = Vector3.Distance(enemyAgent.transform.position, collider.transform.position);
                    var temp = collider.GetComponent<SoliderAgent>();
                    if (!CheckMatchAttackType(temp))
                    {
                        continue;
                    }
                    var tempTarget = new AttackSoliderTarget(tempDis,temp);
                    tempAttackTargets.Add(tempTarget);
                }
                SortMultiTargetsByDistance(tempAttackTargets);
                for (int i = 0; i < enemyModel.attackNum; i++)
                {
                    attackTargets.Add(tempMultiTarget[i]);
                }
            }
        }

        private bool CheckMatchAttackType(SoliderAgent target)
        {
            //若attackEnemyType是多种，那么----待扩展
            if (target.soliderModel.soliderType != enemyModel.attackSoliderType )
            {
                return false;
            }

            return true;
        }

        private void SortMultiTargetsByDistance(List<AttackSoliderTarget> attackTargets)
        {
            attackTargets.Sort((a, b) => a.dis.CompareTo(b.dis));
        }

        #endregion
    }
}