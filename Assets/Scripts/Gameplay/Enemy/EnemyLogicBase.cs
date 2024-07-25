using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Enemy
{
    public class EnemyLogicBase
    {
        
        private EnemyAgent enemyAgent;
        private EnemyModelBase enemyModel;

        private const float frontCheckDistance = 2f;
        private List<EnemyAgent> attackTargets = new List<EnemyAgent>();
        

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
                    LayerMask.GetMask("Enemy"));

            int enemyCount = 0;
            foreach (var hitCollider in hitColliders)
            {
                // 过滤掉自己的碰撞体
                if (hitCollider.gameObject != enemyAgent.gameObject)
                {
                    enemyCount++;
                    Debug.Log("Enemy detected: " + hitCollider.gameObject.name);
                    // 在这里可以实现攻击逻辑
                }
            }

            if (enemyCount <= 0)
            {
                Debug.Log("附近没敌人");
                return false;
            }

            Debug.Log($"附近有{enemyCount}个敌人");
            return true;
        }

        private void ClearTarget()
        {
            attackTargets.Clear();
        }


        private void GetTarget()
        {
            //每次获取新的目标前，先把已有的目标清除
            ClearTarget();

            Collider[] hitColliders =
                Physics.OverlapSphere(enemyAgent.transform.position, enemyModel.attackRange,
                    LayerMask.GetMask("Enemy"));

            //若是单攻
            var minDis = 10000f;
            EnemyAgent tempSingleTarget = null;
            List<EnemyAgent> tempMultiTarget = new List<EnemyAgent>();
            if (enemyModel.attackNum == 1)
            {
                foreach (var collider in hitColliders)
                {
                    var tempDis = Vector3.Distance(enemyAgent.transform.position, collider.transform.position);
                    if (tempDis <= minDis)
                    {
                        minDis = tempDis;
                        tempSingleTarget = collider.GetComponent<EnemyAgent>();
                    }
                }

                attackTargets.Add(tempSingleTarget);
            }

            //若是群攻
            else
            {
                foreach (var collider in hitColliders)
                {
                    var tempDis = Vector3.Distance(enemyAgent.transform.position, collider.transform.position);
                    if (tempDis <= minDis)
                    {
                        minDis = tempDis;
                        tempSingleTarget = collider.GetComponent<EnemyAgent>();
                    }


                    tempMultiTarget.Insert(0, tempSingleTarget);
                }

                for (int i = 0; i < enemyModel.attackNum; i++)
                {
                    attackTargets.Add(tempMultiTarget[i]);
                }
            }
        }

        #endregion
    }
}