using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Player;
using ScriptableObjects.SoliderStateTypeSO;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

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
        private List<SoliderAgent> distanceTargets = new List<SoliderAgent>(); //自己的攻击目标---base distance
        private List<SoliderAgent> focusTargets = new List<SoliderAgent>();    //自己的攻击目标---base focus
        private HashSet<SoliderAgent> attackers = new HashSet<SoliderAgent>(); //在对自己攻击的士兵
        

        //攻击
        private float attackTimer = 1000f;
        public bool isAttackReady = true;

        //阻挡
        public HashSet<SoliderAgent> blockSoilders = new HashSet<SoliderAgent>();

        //血量
        public float curHp;
        //BuffManager
        protected BuffManager enemyBuffManager;
        protected EnemyLogicBase(EnemyAgent agent)
        {
            enemyAgent = agent;
            enemyModel = enemyAgent.enemyModel;
            curHp = enemyModel.maxHp;

            enemyBuffManager = new BuffManager(enemyAgent);
        }

        public void RemoveTarget(SoliderAgent target)
        {
            if (distanceTargets.Contains(target))
            {
                distanceTargets.Remove(target);
                Debug.Log($"Distance Targets removed: {target.soliderModel.soliderName}");
            }
            else
            {
                Debug.Log("Distance Targets not found in the list.");
            }

            if (focusTargets.Contains(target))
            {
                focusTargets.Remove(target);
                Debug.Log($"Focus Target removed: {target.soliderModel.soliderName}");
            }
            else
            {
                Debug.Log("Focus Target not found in the list.");
            }
        }

        private void DrawRange(EnemyAgent enemyAgent, float range)
        {
            Vector3 start = enemyAgent.transform.position;
            Vector3 end = start + Vector3.up * 0.1f;

            int segments = 20;
            float angle = 0f;
            float angleStep = 360f / segments;
            for (int i = 0; i < segments; i++)
            {
                Vector3 offset = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle)) *
                                 range;
                Vector3 nextOffset = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angle + angleStep)), 0,
                    Mathf.Cos(Mathf.Deg2Rad * (angle + angleStep))) * range;

                Debug.DrawLine(start + offset, start + nextOffset, Color.blue);

                angle += angleStep;
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
                Physics.OverlapSphere(enemyAgent.transform.position, enemyModel.attackRange,
                    LayerMask.GetMask("Solider"));

            int soliderCount = 0;
            foreach (var hitCollider in hitColliders)
            {
                // 过滤掉自己的碰撞体
                if (hitCollider.gameObject != enemyAgent.gameObject)
                {
                    soliderCount++;
                    // 在这里可以实现攻击逻辑
                }
            }

            if (soliderCount <= 0)
            {
                return false;
            }

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
                Vector3 offset = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle)) *
                                 enemyModel.attackRange;
                Vector3 nextOffset = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angle + angleStep)), 0,
                    Mathf.Cos(Mathf.Deg2Rad * (angle + angleStep))) * enemyModel.attackRange;

                Debug.DrawLine(start + offset, start + nextOffset, Color.red);

                angle += angleStep;
            }
        }

        private void ClearTarget()
        {
            distanceTargets.Clear();
        }

        //GetTarget需要每帧调吗，会不会很耗性能
        public virtual void GetTarget()
        {
            //每次获取新的目标前，先把已有的目标清除
            ClearTarget();
            //子类override
        }


        protected void DistanceBasedEnemyGetTarget()
        {
            //List<SoliderAgent> tempMultiTarget = new List<SoliderAgent>();
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
                distanceTargets.Add(tempDistanceTargets[i].target);
            }
        }

        public void GetFocusTarget()
        {
            if (HasFocusTarget())
                return;

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

                var tempTarget = new AttackSoliderTarget(tempDis, temp);
                tempDistanceTargets.Add(tempTarget);
            }
            SortMultiTargetsByDistance(tempDistanceTargets);
            for (int i = 0; i < enemyModel.attackNum - focusTargets.Count ; i++)
            {
                focusTargets.Add(tempDistanceTargets[i].target);
            }
        }

        public bool HasFocusTarget()
        {
            if (focusTargets.Count >= enemyModel.attackNum )
                return true;
            return false;
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

        private void SortMultiTargetsByDistance(List<AttackSoliderTarget> distanceTargets)
        {
            distanceTargets.Sort((a, b) => a.dis.CompareTo(b.dis));
        }

        #endregion

        #region 攻击

        public virtual void Attack()
        {
        }
        
        public void MeleeAOE()
        {
            if (isAttackReady)
            {
                CalculateCd();
                for (int i = enemyAgent.enemyLogic.focusTargets.Count - 1; i >= 0; i--)
                {
                    Debug.Log("单体AOE攻击！！！");
                    enemyAgent.enemyLogic.focusTargets[i].soliderLogic.OnTakeAOEDamage(
                        enemyAgent.enemyModel.attackPoint,
                        enemyAgent.enemyModel.magicAttackPoint, 
                        enemyAgent,
                        enemyAgent.enemyModel.attackAoeRange);
                }

            }
        }
        
        protected async void CalculateCd()
        {
            isAttackReady = false;
            await Task.Delay(TimeSpan.FromSeconds(enemyModel.attackInterval));
            isAttackReady = true;
        }

        //最基础的近战
        protected void MeleeAttack()
        {
            if (isAttackReady)
            {
                for (int i = enemyAgent.enemyLogic.distanceTargets.Count-1; i >= 0; i--)
                {
                    enemyAgent.enemyLogic.distanceTargets[i].soliderLogic.OnTakeDamage(enemyAgent.enemyModel.attackPoint,
                        enemyAgent.enemyModel.magicAttackPoint, enemyAgent);
                    Debug.Log("敌人攻击距离最近");
                }

                CalculateCd();
            }
        }

        protected void FocusAttack()
        {
            if (isAttackReady)
            {
                for (int i = enemyAgent.enemyLogic.focusTargets.Count-1; i >= 0; i--)
                {
                    enemyAgent.enemyLogic.focusTargets[i].soliderLogic.OnTakeDamage(enemyAgent.enemyModel.attackPoint,
                        enemyAgent.enemyModel.magicAttackPoint, enemyAgent);
                    Debug.Log("敌人专注攻击");
                }
                // enemyAgent.enemyLogic.focusTarget.soliderLogic.OnTakeDamage(enemyAgent.enemyModel.attackPoint,
                //     enemyAgent.enemyModel.magicAttackPoint, enemyAgent);
                // Debug.Log("敌人专注攻击");

                CalculateCd();
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
            curHp -= enemyBuffManager.CalculateDamage(new DamageInfo(soliderAgent, enemyAgent));
            // enemyAgent.enemyModel.maxHp = enemyAgent.enemyModel.maxHp -
            //                               (damage * (1 - enemyModel.defendReducePercent)) -
            //                               (magicDamage * (1 - enemyModel.magicDefendReducePercent));

            Debug.Log("敌人目前的血量是：" +curHp);
            // Debug.Log("造成的物理伤害为：" + (damage * (1 - enemyModel.defendReducePercent)));
            // Debug.Log("造成的法术伤害为：" + (magicDamage * (1 - enemyModel.magicDefendReducePercent)));

            enemyAgent.StartCoroutine(FlashRed());


            if (curHp <= 0)
            {
                Die();
            }
        }

        public List<EnemyAgent> GetAOETargets(Vector3 position, float aoeRange)
        {
            List<EnemyAgent> aoeTargets = new List<EnemyAgent>();
            Collider[] hitColliders = Physics.OverlapSphere(position, aoeRange, LayerMask.GetMask("Enemy"));

            foreach (var collider in hitColliders)
            {
                var enemy = collider.GetComponent<EnemyAgent>();
                if (enemy != null && enemy != enemyAgent) // 确保不是当前的敌人
                {
                    aoeTargets.Add(enemy);
                }
            }

            return aoeTargets;
        }


        public void OnTakeAOEDamage(float damage, float magicDamage, SoliderAgent soliderAgent, float aoeRange)
        {
            // 当前敌人受到伤害
            OnTakeDamage(damage, magicDamage, soliderAgent);

            // 获取 aoeRange 范围内的所有敌人
            List<EnemyAgent> aoeTargets = GetAOETargets(enemyAgent.transform.position, aoeRange);

            if(aoeTargets != null)
            {
                Debug.Log("不为空");
            }
            else
            {
                Debug.Log("为空");
                foreach(var item in aoeTargets)
                {
                    Debug.Log(item.gameObject.name);
                }
            }

            foreach (var enemy in aoeTargets)
            {
                enemy.enemyLogic.OnTakeDamage(damage, magicDamage, soliderAgent);
            }

            DrawRange(enemyAgent, aoeRange);
        }


        
        private void AddAttacker(SoliderAgent attacker)
        {
            if (!attacker)
            {
                return;
            }
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

            //通知在打他的士兵，他死了
            foreach (var agent in attackers)
            {
                agent.soliderLogic.RemoveTarget(enemyAgent);
            }

            //通知被他阻挡的士兵，他死了
            foreach (var agent in blockSoilders)
            {
                agent.soliderLogic.blocker = null;
            }

            enemyAgent.StopAllCoroutines();
            Object.Destroy(enemyAgent.gameObject);
        }
    }
}