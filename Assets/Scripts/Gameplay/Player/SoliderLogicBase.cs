using System.Collections.Generic;
using Gameplay.Enemy;
using UnityEngine;

namespace Gameplay.Player
{
    public class SoliderLogicBase
    {
        private SoliderAgent soliderAgent;
        private SoliderModelBase soliderModel;

        private const float frontCheckDistance = 2f;
        private List<EnemyAgent> attackTargets = new List<EnemyAgent>();


        //移动路径
        private Transform moveTarget;
        private int waypointIndex = 0;
        private Transform[] pathPoints;


        public SoliderLogicBase(SoliderAgent agent)
        {
            soliderAgent = agent;
            soliderModel = soliderAgent.soliderModel;
        }

        
        //移动
        public void SetPath(int pathIndex)
        {
            if (pathIndex < 0 || pathIndex >= Waypoints.paths.Count)
            {
                Debug.LogError("Invalid path index");
                return;
            }

            pathPoints = Waypoints.paths[pathIndex];
            waypointIndex = 0;
            moveTarget = pathPoints[0];
        }

        public void Move()
        {
            if (pathPoints == null || pathPoints.Length == 0) return;

            Vector3 dir = moveTarget.position - soliderAgent.transform.position;
            soliderAgent.transform.Translate(soliderModel.moveSpeed * Time.deltaTime * dir.normalized, Space.World);

            if (Vector3.Distance(soliderAgent.transform.position, moveTarget.position) <= 0.4f)
            {
                GetNextWaypoint();
            }
        }

        private void GetNextWaypoint()
        {
            if (waypointIndex >= pathPoints.Length - 1)
            {
                EndPath();
                return;
            }

            waypointIndex++;
            moveTarget = pathPoints[waypointIndex];
        }

        private void EndPath()
        {
            // PlayerStats.Lives--;
            // WaveSpawner.EnemiesAlive--;
            // Destroy(gameObject);
        }
        
        //判断障碍
        public bool CheckObstacle()
        {
            RaycastHit hit;
            if (Physics.Raycast(soliderAgent.transform.position, soliderAgent.transform.forward, out hit, frontCheckDistance))
            {
                if (hit.collider.CompareTag("Obstacle"))
                {
                    return true;
                }
            }

            return false;
        }
        
        

        public void Stop()
        {
            
        }
        
        #region 攻击判定

        public bool CheckCanAttack()
        {
            Collider[] hitColliders =
                Physics.OverlapSphere(soliderAgent.transform.position, frontCheckDistance,
                    LayerMask.GetMask("Enemy"));

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
                Physics.OverlapSphere(soliderAgent.transform.position, soliderModel.attackRange,
                    LayerMask.GetMask("Enemy"));

            //若是单攻
            var minDis = 10000f;
            EnemyAgent tempSingleTarget = null;
            List<EnemyAgent> tempMultiTarget = new List<EnemyAgent>();
            if (soliderModel.attackNum == 1)
            {
                foreach (var collider in hitColliders)
                {
                    var tempDis = Vector3.Distance(soliderAgent.transform.position, collider.transform.position);
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
                    var tempDis = Vector3.Distance(soliderAgent.transform.position, collider.transform.position);
                    if (tempDis <= minDis)
                    {
                        minDis = tempDis;
                        tempSingleTarget = collider.GetComponent<EnemyAgent>();
                    }


                    tempMultiTarget.Insert(0, tempSingleTarget);
                }

                for (int i = 0; i < soliderModel.attackNum; i++)
                {
                    attackTargets.Add(tempMultiTarget[i]);
                }
            }
        }

        #endregion
    }
}