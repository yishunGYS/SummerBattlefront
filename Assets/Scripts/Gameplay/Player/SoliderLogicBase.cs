using System.Collections;
using System.Collections.Generic;
using Gameplay.Enemy;
using UnityEngine;

namespace Gameplay.Player
{
    class AttackEnemyTarget
    {
        public float dis;
        public EnemyAgent target;

        public AttackEnemyTarget(float dis,EnemyAgent target)
        {
            this.dis = dis;
            this.target = target;
        }
    }
    public class SoliderLogicBase
    {
        private SoliderAgent soliderAgent;
        private SoliderModelBase soliderModel;

        private const float frontCheckDistance = 2f;
        public List<EnemyAgent> attackTargets = new List<EnemyAgent>();


        //移动路径
        private Transform moveTarget;
        private int waypointIndex = 0;
        private Transform[] pathPoints;

        public SoliderLogicBase(SoliderAgent agent)
        {
            soliderAgent = agent;
            soliderModel = soliderAgent.soliderModel;
        }

        public void RemoveTarget(EnemyAgent target)
        {
            if (attackTargets.Contains(target))
            {
                attackTargets.Remove(target);
                Debug.Log($"Target removed: {target.enemyModel.enemyName}");
            }
            else
            {
                Debug.Log("Target not found in the list.");
            }
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
            if (pathPoints == null || pathPoints.Length == 0) return false;

            Vector3 dir = moveTarget.position - soliderAgent.transform.position;
            RaycastHit hit;

            if (soliderAgent == null)
            {
                Debug.LogError("soliderAgent is not initialized.");
                return false;
            }

            Debug.DrawRay(soliderAgent.transform.position, dir.normalized * frontCheckDistance, Color.red);

            if (Physics.Raycast(soliderAgent.transform.position, dir.normalized, out hit, frontCheckDistance))
            {
                EnemyAgent enemyAgent = hit.collider.gameObject.GetComponent<EnemyAgent>();
                if (enemyAgent != null)
                {
                    int blockNum = enemyAgent.enemyModel.blockNum;
                    if (blockNum > 0)
                    {
                        // 可以被阻挡，减少 blockNum
                        enemyAgent.enemyModel.blockNum--;
                        return true;
                    }
                    else
                    {
                        // 不能被阻挡，blockNum 小于等于零
                        return false;
                    }
                }
                else if (hit.collider.CompareTag("Obstacle"))
                {
                    return true; // 其他障碍物阻挡
                }
            }

            return false;
        }


        public void Stop()
        {
            
        }

        public bool CheckCanBeBlock()
        {
            return true;
        }

        #region 攻击判定

        public bool CheckCanAttack()
        {
            // 绘制攻击判定范围的可视化效果
            DrawAttackRange();

            Collider[] hitColliders =
                Physics.OverlapSphere(soliderAgent.transform.position, soliderModel.attackRange,
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

        private void DrawAttackRange()
        {
            Vector3 start = soliderAgent.transform.position;
            Vector3 end = start + Vector3.up * 0.1f;

            int segments = 20;
            float angle = 0f;
            float angleStep = 360f / segments;
            for (int i = 0; i < segments; i++)
            {
                Vector3 offset = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle)) * soliderModel.attackRange;
                Vector3 nextOffset = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angle + angleStep)), 0, Mathf.Cos(Mathf.Deg2Rad * (angle + angleStep))) * soliderModel.attackRange;

                Debug.DrawLine(start + offset, start + nextOffset, Color.blue);

                angle += angleStep;
            }
        }

        private void ClearTarget()
        {
            attackTargets.Clear();
        }


        public void GetTarget()
        {
            //每次获取新的目标前，先把已有的目标清除
            ClearTarget();

            Collider[] hitColliders =
                Physics.OverlapSphere(soliderAgent.transform.position, soliderModel.attackRange,
                    LayerMask.GetMask("Enemy"));

            //若是单攻
            var minDis = 10000f;
            EnemyAgent singleTarget = null;
            List<EnemyAgent> tempMultiTarget = new List<EnemyAgent>();
            List<AttackEnemyTarget> tempAttackTargets = new List<AttackEnemyTarget>();
            if (soliderModel.attackNum == 1)
            {
                foreach (var collider in hitColliders)
                {
                    var tempDis = Vector3.Distance(soliderAgent.transform.position, collider.transform.position);
                    var temp = collider.GetComponent<EnemyAgent>();
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
            else if(soliderModel.attackNum > 1)
            {
                foreach (var collider in hitColliders)
                {
                    var tempDis = Vector3.Distance(soliderAgent.transform.position, collider.transform.position);
                    var temp = collider.GetComponent<EnemyAgent>();
                    if (!CheckMatchAttackType(temp))
                    {
                        continue;
                    }
                    var tempTarget = new AttackEnemyTarget(tempDis,temp);
                    tempAttackTargets.Add(tempTarget);
                }
                SortMultiTargetsByDistance(tempAttackTargets);
                for (int i = 0; i < soliderModel.attackNum; i++)
                {
                    attackTargets.Add(tempMultiTarget[i]);
                }
            }
        }

        private bool CheckMatchAttackType(EnemyAgent target)
        {
            //todo 若attackEnemyType是多种，那么----待扩展
            if (target.enemyModel.enemyType != soliderModel.attackEnemyType )
            {
                return false;
            }

            return true;
        }

        private void SortMultiTargetsByDistance(List<AttackEnemyTarget> attackTargets)
        {
            attackTargets.Sort((a, b) => a.dis.CompareTo(b.dis));
        }

        #endregion

        public void OnTakeDamage(float damage, float magicDamage)
        {
            // 减少士兵的生命值
            soliderModel.maxHp = soliderModel.maxHp - (damage * (1 - soliderModel.defendReducePercent)) - (magicDamage * (1 - soliderModel.magicDefendReducePercent));

            Debug.Log("士兵目前的血量是：" + soliderModel.maxHp);
            Debug.Log("士兵受到的物理伤害为：" + (damage * (1 - soliderModel.defendReducePercent)));
            Debug.Log("士兵受到的法术伤害为：" + (magicDamage * (1 - soliderModel.magicDefendReducePercent)));

            soliderAgent.StartCoroutine(FlashRed());

            if (soliderModel.maxHp <= 0)
            {
                Die();
            }
        }

        private IEnumerator FlashRed()
        {
            Renderer renderer = soliderAgent.GetComponent<Renderer>();
            if (renderer == null)
            {
                Debug.LogError("Renderer component not found.");
                yield break;
            }

            Color originalColor = renderer.material.color;
            renderer.material.color = Color.red;

            yield return new WaitForSeconds(0.1f); // 控制闪烁时间

            renderer.material.color = originalColor;
        }

        private void Die()
        {
            Debug.Log($"{soliderModel.soliderName} has died!");

            // 播放死亡动画或特效
            // 可以在这里添加更多死亡处理逻辑，例如从场景中移除士兵，更新分数等

            Object.Destroy(soliderAgent.gameObject);
        }

    }
}