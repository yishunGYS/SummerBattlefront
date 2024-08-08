using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _3DlevelEditor_GYS;
using Gameplay.Enemy;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Gameplay.Player
{
    class AttackEnemyTarget
    {
        public float dis;
        public EnemyAgent target;

        public AttackEnemyTarget(float dis, EnemyAgent target)
        {
            this.dis = dis;
            this.target = target;
        }
    }

    public class SoliderLogicBase
    {
        protected SoliderAgent soliderAgent;
        protected SoliderModelBase soliderModel;

        private const float frontCheckDistance = 2f;

        public List<UnitAgent> attackTargets = new List<UnitAgent>(); //在攻击的目标:可以是敌人也可以是友方
        private HashSet<EnemyAgent> attackers = new HashSet<EnemyAgent>(); //在对自己攻击的敌方

        //攻击
        private float attackTimer = 1000f;
        public bool isAttackReady = true;

        //血量
        public int curHp;

        //阻挡的敌人
        public EnemyAgent blocker;

        //用来获取Block
        public GridCell currentBlock;
        public List<GridCell> nextBlock;

        public void InitBlockData(GridCell block)
        {
            currentBlock = block;
            nextBlock = block.nextCells;
        }

        //BuffManager
        public BuffManager playerBuffManager;

        public SoliderLogicBase(SoliderAgent agent)
        {
            soliderAgent = agent;
            soliderModel = soliderAgent.soliderModel;
        }


        #region 移动

        public void Move()
        {
            if (currentBlock == null || nextBlock == null || nextBlock.Count != 1) return;

            var nextTarget = nextBlock[0];
            var nextPoint = (nextTarget.transform.position + new Vector3(0f, nextTarget.transform.localScale.y, 0f));

            Vector3 dir = nextPoint - soliderAgent.transform.position;

            soliderAgent.transform.Translate(soliderModel.moveSpeed * Time.deltaTime * dir.normalized, Space.World);

            if (Vector3.Distance(soliderAgent.transform.position, nextPoint) <= 0.4f)
            {
                GetNextBlcok();
            }
        }


        private void GetNextBlcok()
        {
            currentBlock = nextBlock[0];
            nextBlock = currentBlock.nextCells;

            if (currentBlock == null || nextBlock == null || nextBlock.Count != 1)
            {
                if (BlockManager.instance.headSoliderBlocks.ContainsKey(soliderAgent))
                {
                    BlockManager.instance.OnHeadSoliderDestory(soliderAgent);
                }

                GameObject.Destroy(soliderAgent.gameObject);
                return;
            }
        }

        private void EndPath()
        {
            // PlayerStats.Lives--;
            // WaveSpawner.EnemiesAlive--;
            // Destroy(gameObject);
        }

        #endregion

        #region 判断障碍

        public bool CheckObstacle()
        {
            if (blocker != null)
            {
                return true;
            }

            if (currentBlock == null || nextBlock == null || nextBlock.Count != 1) return false;

            Vector3 dir = nextBlock[0].transform.position - soliderAgent.transform.position;
            RaycastHit hit;
            Debug.DrawRay(soliderAgent.transform.position, dir.normalized * frontCheckDistance, Color.red);

            if (Physics.Raycast(soliderAgent.transform.position, dir.normalized, out hit, frontCheckDistance))
            {
                EnemyAgent enemyAgent = hit.collider.gameObject.GetComponent<EnemyAgent>();
                if (enemyAgent != null)
                {
                    int enemyBlockNum = enemyAgent.enemyModel.blockNum;
                    if (enemyAgent.enemyLogic.blockSoilders.Count < enemyBlockNum &&
                        !enemyAgent.enemyLogic.blockSoilders.Contains(soliderAgent))
                    {
                        // 可以被阻挡，
                        enemyAgent.enemyLogic.blockSoilders.Add(soliderAgent);
                        blocker = enemyAgent;
                        return true;
                    }
                    else
                    {
                        // 不能被阻挡
                        return false;
                    }
                }
                // else if (hit.collider.CompareTag("Enemy"))
                // {
                //     return true; // 其他障碍物阻挡
                // }
            }

            return false;
        }

        #endregion

        //有些士兵会穿透
        public virtual bool CheckCanBeBlock()
        {
            return true;
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


        //有攻击目标
        public bool HasAttackTarget()
        {
            // 绘制攻击判定范围的可视化效果
            DrawAttackRange();

            Collider[] hitColliders =
                Physics.OverlapSphere(soliderAgent.transform.position, soliderModel.attackRange,
                    LayerMask.GetMask("Enemy"));

            if (hitColliders.Length <= 0)
            {
                return false;
            }

            return true;
        }


        protected virtual void ClearTarget()
        {
        }

        public virtual void RemoveTarget(UnitAgent target)
        {
        }

        public virtual void GetTarget()
        {
            //每次获取新的目标前，先把已有的目标清除
            ClearTarget();
            //子类override
        }


        protected virtual bool CheckMatchAttackType(EnemyAgent target)
        {
            return true;
        }

        #endregion


        #region 攻击

        public virtual void Attack()
        {
        }

        protected async void CalculateCd()
        {
            isAttackReady = false;
            //攻击间隔<0 说明只能单次攻击
            if (soliderModel.attackInterval<0)
            {
                return;
            }
            
            
            await Task.Delay(TimeSpan.FromSeconds(soliderModel.attackInterval));
            isAttackReady = true;
        }


        //远程写在子类
        protected void RangeAttack()
        {
        }

        #endregion


        #region 受击

        public void OnTakeDamage(EnemyAgent enemyAgent)
        {
            AddAttacker(enemyAgent);

            var damagePoint = playerBuffManager.CalculateDamage(new DamageInfo(enemyAgent, soliderAgent));
            if (damagePoint == 0)
            {
                Debug.Log("士兵免伤，目前的血量是：" + curHp);
                return;
            }

            Debug.Log("士兵扣血，目前的血量是：" + curHp);
            curHp -= damagePoint;
            soliderAgent.StartCoroutine(FlashRed());
            if (soliderModel.maxHp <= 0)
            {
                Die();
            }
        }
        //受到AOE伤害后,根据当前的攻击者(敌人)的AOE攻击范围,以自己为中心寻找范围内的士兵,并使其造成伤害
        public void OnTakeAOEDamage(EnemyAgent enemyAgent, float aoeRange)
        {
            // 当前敌人受到伤害
            OnTakeDamage(enemyAgent);

            // 获取 aoeRange 范围内的所有敌人
            List<SoliderAgent> aoeTargets = GetAOETargets(soliderAgent.transform.position, aoeRange);

            if (aoeTargets != null)
            {
                Debug.Log("不为空");
            }
            else
            {
                Debug.Log("为空");
                foreach (var item in aoeTargets)
                {
                    Debug.Log(item.gameObject.name);
                }
            }

            foreach (var solider in aoeTargets)
            {
                solider.soliderLogic.OnTakeDamage(enemyAgent);
            }

            DrawRange(enemyAgent, aoeRange);
        }

        public List<SoliderAgent> GetAOETargets(Vector3 position, float aoeRange)
        {
            List<SoliderAgent> aoeTargets = new List<SoliderAgent>();
            Collider[] hitColliders = Physics.OverlapSphere(position, aoeRange, LayerMask.GetMask("Solider"));

            foreach (var collider in hitColliders)
            {
                var solider = collider.GetComponent<SoliderAgent>();
                if (solider != null && solider != soliderAgent) // 确保不是当前的敌人
                {
                    aoeTargets.Add(solider);
                }
            }

            return aoeTargets;
        }

        #endregion


        private void AddAttacker(EnemyAgent attacker)
        {
            if (!attackers.Contains(attacker))
            {
                attackers.Add(attacker);
                Debug.Log($"{attacker.enemyModel.enemyName} started attacking Me!");
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


        //召唤
        public virtual void Summon()
        {
        }

        //死亡
        public virtual void Die()
        {
            Debug.Log($"{soliderModel.soliderName} has died!");
            //通知在打他的敌人，他死了
            foreach (var agent in attackers)
            {
                agent.enemyLogic.RemoveTarget(soliderAgent);
            }

            //若该士兵是被阻挡的，通知被阻挡的人，他死了
            if (blocker != null)
            {
                blocker.enemyLogic.blockSoilders.Remove(soliderAgent);
            }

            soliderAgent.StopAllCoroutines();
            Object.Destroy(soliderAgent.gameObject);
        }


        #region 绘制

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

        private void DrawAttackRange()
        {
            Vector3 start = soliderAgent.transform.position;
            Vector3 end = start + Vector3.up * 0.1f;

            int segments = 20;
            float angle = 0f;
            float angleStep = 360f / segments;
            for (int i = 0; i < segments; i++)
            {
                Vector3 offset = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle)) *
                                 soliderModel.attackRange;
                Vector3 nextOffset = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (angle + angleStep)), 0,
                    Mathf.Cos(Mathf.Deg2Rad * (angle + angleStep))) * soliderModel.attackRange;

                Debug.DrawLine(start + offset, start + nextOffset, Color.blue);

                angle += angleStep;
            }
        }

        #endregion
    }
}