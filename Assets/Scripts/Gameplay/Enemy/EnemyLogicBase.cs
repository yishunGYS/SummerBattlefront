using System.Collections;
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
        private List<SoliderAgent> attackTargets = new List<SoliderAgent>(); //自己的攻击目标
        private HashSet<SoliderAgent> attackers = new HashSet<SoliderAgent>(); //在对自己攻击的士兵

        //攻击
        private float attackTimer;
        public bool isAttackReady = true;

        public EnemyLogicBase(EnemyAgent agent)
        {
            enemyAgent = agent;
            enemyModel = enemyAgent.enemyModel;
        }

        public void RemoveTarget(SoliderAgent target)
        {
            if (attackTargets.Contains(target))
            {
                attackTargets.Remove(target);
                Debug.Log($"Target removed: {target.soliderModel.soliderName}");
            }
            else
            {
                Debug.Log("Target not found in the list.");
            }
        }


        #region 攻击判定

        public bool CheckCanAttack()
        {
            if (HasAttackTarget() && isAttackReady)
            {
                return true;
            }

            return false;
        }

        public bool HasAttackTarget()
        {
            // 绘制攻击判定范围的可视化效果
            DrawAttackRange();

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
                    Debug.Log("Solider detected: " + hitCollider.gameObject.name);
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

        private void DrawAttackRange()
        {
            Vector3 start = enemyAgent.transform.position;
            int segments = 20;
            float angle = 0f;
            float angleStep = 360f / segments;

            for (int i = 0; i < segments; i++)
            {
                float rad = Mathf.Deg2Rad * angle;
                float nextRad = Mathf.Deg2Rad * (angle + angleStep);

                Vector3 point1 = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * frontCheckDistance + start;
                Vector3 point2 = new Vector3(Mathf.Sin(nextRad), 0, Mathf.Cos(nextRad)) * frontCheckDistance + start;

                Debug.DrawLine(point1, point2, Color.red);

                angle += angleStep;
            }
        }

        private void ClearTarget()
        {
            attackTargets.Clear();
        }

        //GetTarget需要每帧调吗，会不会很耗性能
        public virtual void GetTarget()
        {
            //每次获取新的目标前，先把已有的目标清除
            ClearTarget();
            //子类override
            
        }

        //若是单攻
        protected void SingleAttackEnemyGetTarget()
        {
            var minDis = 10000f;
            SoliderAgent singleTarget = null;
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

                if (tempDis <= minDis)
                {
                    minDis = tempDis;
                    singleTarget = temp;
                }
            }

            attackTargets.Add(singleTarget);
        }


        protected void MultiAttackEnemyGetTarget()
        {
            List<SoliderAgent> tempMultiTarget = new List<SoliderAgent>();
            List<AttackSoliderTarget> tempAttackTargets = new List<AttackSoliderTarget>();
            
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
                tempAttackTargets.Add(tempTarget);
            }

            SortMultiTargetsByDistance(tempAttackTargets);
            for (int i = 0; i < enemyModel.attackNum; i++)
            {
                attackTargets.Add(tempMultiTarget[i]);
            }
        }

        //辅助/治疗获取目标
        protected void AssistEnemyGetTarget()
        {
            
        }

        
        private bool CheckMatchAttackType(SoliderAgent target)
        {
            //若attackEnemyType是多种，那么----待扩展
            if (target.soliderModel.soliderType != enemyModel.attackSoliderType)
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

        #region 攻击

        public virtual void Attack()
        {
            CalculateCd();
        }

        private void CalculateCd()
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= enemyModel.attackInterval)
            {
                isAttackReady = true;
                attackTimer = 0f; // 重置计时器
            }
            else
            {
                isAttackReady = false;
            }
        }

        //最基础的近战
        protected void MeleeAttack()
        {
            if (isAttackReady)
            {
                foreach (var target in enemyAgent.enemyLogic.attackTargets)
                {
                    target.soliderLogic.OnTakeDamage(enemyAgent.enemyModel.attackPoint,
                        enemyAgent.enemyModel.magicAttackPoint, enemyAgent);
                }
            }
        }


        //最基础的远战
        protected void RangeAttack()
        {
            
        }

        #endregion
        
        
        public void OnTakeDamage(float damage, float magicDamage, SoliderAgent soliderAgent)
        {
            AddAttacker(soliderAgent);
            // 减少敌人的生命值
            enemyAgent.enemyModel.maxHp = enemyAgent.enemyModel.maxHp -
                                          (damage * (1 - enemyModel.defendReducePercent)) -
                                          (magicDamage * (1 - enemyModel.magicDefendReducePercent));

            Debug.Log("敌人目前的血量是：" + enemyAgent.enemyModel.maxHp);
            Debug.Log("造成的物理伤害为：" + (damage * (1 - enemyModel.defendReducePercent)));
            Debug.Log("造成的法术伤害为：" + (magicDamage * (1 - enemyModel.magicDefendReducePercent)));

            enemyAgent.StartCoroutine(FlashRed());


            if (enemyAgent.enemyModel.maxHp <= 0)
            {
                foreach (var agent in attackers)
                {
                    agent.soliderLogic.RemoveTarget(enemyAgent);
                }

                Die();
            }
        }

        private void AddAttacker(SoliderAgent attacker)
        {
            if (!attackers.Contains(attacker))
            {
                attackers.Add(attacker);
                Debug.Log($"{attacker.soliderModel.soliderName} started attacking Me!");
            }
        }

        private IEnumerator FlashRed()
        {
            Renderer renderer = enemyAgent.GetComponent<Renderer>();
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
            Debug.Log($"{enemyModel.enemyName} has died!");

            // 播放死亡动画或特效
            // 可以在这里添加更多死亡处理逻辑，例如从场景中移除敌人，更新分数等
            enemyAgent.StopAllCoroutines();
            Object.Destroy(enemyAgent.gameObject);
        }
    }
}