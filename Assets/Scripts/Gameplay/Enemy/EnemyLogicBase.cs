using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Features;
using Gameplay.Features.EnemyFeature;
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
        protected EnemyAgent enemyAgent;
        protected EnemyModelBase enemyModel;

        public List<UnitAgent> attackTargets = new List<UnitAgent>(); //自己的攻击目标
        private HashSet<SoliderAgent> attackers = new HashSet<SoliderAgent>(); //在对自己攻击的士兵

        //攻击
        private float attackTimer = 1000f;
        public bool isAttackReady = true;

        //阻挡
        public HashSet<SoliderAgent> blockSoilders = new HashSet<SoliderAgent>();

        //BuffManager
        //public BuffManager enemyBuffManager;

        protected EnemyLogicBase(EnemyAgent agent)
        {
            enemyAgent = agent;
            enemyModel = enemyAgent.enemyModel;
            //curHp = enemyModel.maxHp;

            //enemyBuffManager = new BuffManager(enemyAgent);
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

        protected virtual bool HasAttackTarget()
        {
            // 绘制攻击判定范围的可视化效果
            DrawAttackRange();
            return false;
        }

        
        //GetTarget需要每帧调吗，会不会很耗性能
        public virtual void GetTarget()
        {
            //每次获取新的目标前，先把已有的目标清除[若enemy都是focus target的话先不clear]

            //子类override
        }

        private void ClearTarget()
        {
            //attackTargets.Clear();
        }

        public virtual void RemoveAttackTarget(UnitAgent target)
        {

        }

        public void RemoveBlockTargets(SoliderAgent target)
        {
            if (blockSoilders.Contains(target))
            {
                blockSoilders.Remove(target);
                Debug.Log($"BlockTarget removed: {target.soliderModel.soliderName}");
            }
            else
            {
                Debug.Log("BlockTarget not found in the list.");
            }
        }
        #endregion
        

        #region 攻击

        public virtual void Attack()
        {
        }

        protected async void CalculateCd()
        {
            isAttackReady = false;
            await Task.Delay(TimeSpan.FromSeconds(enemyModel.attackInterval));
            isAttackReady = true;
        }

        #endregion


        #region 受击

        public void OnTakeDamage(SoliderAgent soliderAgent)
        {
            AddAttacker(soliderAgent);
            var damagePoint = enemyAgent.buffManager.CalculateDamage(new DamageInfo(soliderAgent, enemyAgent));
            if (damagePoint == 0)
            {
                Debug.Log("敌人免伤，目前的血量是：" + enemyAgent.curHp);
                return;
            }

            Debug.Log("敌人扣血，敌人目前的血量是：" + enemyAgent.curHp);
            enemyAgent.curHp -= damagePoint;
            enemyAgent.StartCoroutine(FlashRed());
            if (enemyAgent.curHp <= 0)
            {
                var eliteCmpt = enemyAgent.GetComponent<EliteEnemyFeature>();
                if (eliteCmpt)
                {
                    eliteCmpt.lastCamp = soliderAgent.soliderLogic.birthPoint;
                    eliteCmpt.lastPath = soliderAgent.soliderLogic.birthCell;
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
        
        private List<EnemyAgent> GetAOETargets(Vector3 position, float aoeRange)
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
        
        public void OnTakeAOEDamage(SoliderAgent soliderAgent, float aoeRange)
        {
            // 当前敌人受到伤害
            OnTakeDamage(soliderAgent);

            // 获取 aoeRange 范围内的所有敌人
            List<EnemyAgent> aoeTargets = GetAOETargets(enemyAgent.transform.position, aoeRange);
            foreach (var enemy in aoeTargets)
            {
                enemy.enemyLogic.OnTakeDamage(soliderAgent);
            }

            DrawRange(enemyAgent, aoeRange);
        }

        private IEnumerator FlashRed()
        {
            Renderer renderer = enemyAgent.GetComponent<Renderer>();
            if (renderer == null)
            {
                Debug.LogError("Renderer component not found.");
                yield break;
            }

            var material = renderer.material;
            Color originalColor = material.color;
            material.color = Color.red;

            yield return new WaitForSeconds(0.1f); // 控制闪烁时间

            material.color = originalColor;
        }

        #endregion


        #region 死亡
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
        

        #endregion
        

        #region 绘制

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

        #endregion
    }
}