using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _3DlevelEditor_GYS;
using Gameplay.Enemy;
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
        public float curHp;

        //阻挡的敌人
        public EnemyAgent blocker;

        //用来获取Block
        public GridCell currentBlock;
        public List<GridCell> nextBlock;

        public void InitBlockData(GridCell block)
        {
            currentBlock  = block;
            nextBlock = block.nextCells;
        }
        
        //BuffManager
        public BuffManager playerBuffManager;

        protected SoliderLogicBase(SoliderAgent agent)
        {
            soliderAgent = agent;
            soliderModel = soliderAgent.soliderModel;
            curHp = soliderModel.maxHp;

            playerBuffManager = new BuffManager(soliderAgent);
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
            await Task.Delay(TimeSpan.FromSeconds(soliderModel.attackInterval));
            isAttackReady = true;
        }





        //远程写在子类
        protected void RangeAttack()
        {
        }

        #endregion


        //受击
        public void OnTakeDamage(float damage, float magicDamage, EnemyAgent enemyAgent)
        {
            AddAttacker(enemyAgent);
            // 减少士兵的生命值
            var damageAmout = (int)playerBuffManager.CalculateDamage(new DamageInfo(enemyAgent, soliderAgent));
            curHp -= damageAmout;
            // soliderModel.maxHp = soliderModel.maxHp - (damage * (1 - soliderModel.defendReducePercent)) -
            //                      (magicDamage * (1 - soliderModel.magicDefendReducePercent));

            Debug.Log("士兵目前的血量是：" + curHp);
            // Debug.Log("士兵受到的物理伤害为：" + (damage * (1 - soliderModel.defendReducePercent)));
            // Debug.Log("士兵受到的法术伤害为：" + (magicDamage * (1 - soliderModel.magicDefendReducePercent)));

            if (damageAmout == 0)
            {
                Debug.Log("伤害为0，无敌");
                return;
            }

            soliderAgent.StartCoroutine(FlashRed());

            if (curHp <= 0)
            {
                Die();
            }
        }

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

        
        public void OnUpdateBuff()
        {
            playerBuffManager.UpdateBuff();
        }
    }
}