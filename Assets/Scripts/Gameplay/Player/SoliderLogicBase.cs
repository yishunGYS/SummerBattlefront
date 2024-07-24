using System.Collections.Generic;
using Gameplay.Enemy;
using UnityEngine;

namespace Gameplay.Player
{
    public class SoliderLogicBase
    {
        private SoliderAgent soliderAgent;

        private List<EnemyAgent> targets = new List<EnemyAgent>();
        
        public SoliderLogicBase(SoliderAgent agent)
        {
            soliderAgent = agent;
        }

        public bool CheckCanAttack()
        {
            Collider[] hitColliders =
                Physics.OverlapSphere(soliderAgent.transform.position, soliderAgent.soliderModel.attackRange,LayerMask.GetMask("Enemy"));

            int enemyCount = 0;
            foreach (var hitCollider in hitColliders)
            {
                // 过滤掉自己的碰撞体
                if (hitCollider.gameObject != soliderAgent.gameObject)
                {
                    enemyCount++;
                    Debug.Log("Enemy detected: " + hitCollider.gameObject.name);
                    // 在这里可以实现攻击逻辑
                }
            }

            if (enemyCount<=0)
            {
                Debug.Log("附近没敌人");
                return false;
            }
            Debug.Log($"附近有{enemyCount}个敌人");
            return true;
        }

        private void ClearTarget()
        {
            targets.Clear();
        }


        private void GetTarget()
        {
            //每次获取新的目标前，先把已有的目标清除
            ClearTarget();
            
            Collider[] hitColliders =
                Physics.OverlapSphere(soliderAgent.transform.position, soliderAgent.soliderModel.attackRange,LayerMask.GetMask("Enemy"));

            //若是单攻
            var minDis = 10000f;
            EnemyAgent tempSingleTarget = null;
            List<EnemyAgent> tempMultiTarget = new List<EnemyAgent>();
            if (soliderAgent.soliderModel.attackNum == 1)
            {
                foreach (var collider in hitColliders)
                {
                    var tempDis = Vector3.Distance(soliderAgent.transform.position, collider.transform.position);
                    if (tempDis<=minDis)
                    {
                        minDis = tempDis;
                        tempSingleTarget = collider.GetComponent<EnemyAgent>();
                    }
                }
                
                targets.Add(tempSingleTarget);
            }
            
            //若是群攻
            else
            {
                foreach (var collider in hitColliders)
                {
                    var tempDis = Vector3.Distance(soliderAgent.transform.position, collider.transform.position);
                    if (tempDis<=minDis)
                    {
                        minDis = tempDis;
                        tempSingleTarget = collider.GetComponent<EnemyAgent>();
                    }
                
                    
                    tempMultiTarget.Insert(0,tempSingleTarget);
                }

                for (int i = 0; i < soliderAgent.soliderModel.attackNum; i++)
                {
                    targets.Add(tempMultiTarget[i]);
                }
            }
        }

        public void Stop()
        {
            
        }
    }
    
    
    
}
