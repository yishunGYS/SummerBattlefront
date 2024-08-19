using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _3DlevelEditor_GYS;
using Gameplay.Enemy;
using Managers;
using Systems;
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

        private float frontCheckDistance;

        public List<UnitAgent> attackTargets = new List<UnitAgent>(); //在攻击的目标:可以是敌人也可以是友方
        private HashSet<UnitAgent> attackers = new HashSet<UnitAgent>(); //在对自己攻击的敌方

        //移动
        private Vector3 moveDir;
        
        //攻击
        private float attackTimer = 1000f;
        public bool isAttackReady = true;

        //阻挡的敌人
        public EnemyAgent blocker;

        //用来获取Block
        public GridCell currentBlock;
        public List<GridCell> nextBlock;

        //从哪个出生点出生的
        public StartPoint birthPoint;

        public SoliderLogicBase(SoliderAgent agent)
        {
            soliderAgent = agent;
            soliderModel = soliderAgent.soliderModel;
            //playerBuffManager = new BuffManager(soliderAgent);
            frontCheckDistance = (soliderModel.attackRange<1? soliderModel.attackRange - 0.05f : 1- 0.05f);
        }
        
        public void InitBlockData(GridCell block)
        {
            currentBlock = block;
            nextBlock = block.nextCells;
        }

        public void InitBirthPointData(GridCell block)
        {
            birthPoint = BlockManager.instance.ReturnHeadStartPoint(block);

            if (birthPoint == null)
            {
                birthPoint = currentBlock.previousCells[0].GetComponent<StartPoint>();
            }
        }

        //BuffManager
        //public BuffManager playerBuffManager;




        #region 移动

        public void Move()
        {
            if (currentBlock == null || nextBlock == null || nextBlock.Count != 1) return;

            var nextTarget = nextBlock[0];
            var nextPoint = (nextTarget.transform.position + new Vector3(0f, nextTarget.transform.localScale.y, 0f));

            moveDir= nextPoint - soliderAgent.transform.position;

            soliderAgent.transform.Translate(soliderModel.moveSpeed * Time.deltaTime * moveDir.normalized, Space.World);

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
                // if (BlockManager.instance.headSoliderBlocks.ContainsKey(soliderAgent))
                // {
                //     BlockManager.instance.OnHeadSoliderDestory(soliderAgent);
                // }

                if (currentBlock.gameObject.GetComponent<StartPoint>())
                {
                    Debug.Log("返回费用");
                }
                
                
                

                if (currentBlock.gameObject.GetComponent<EndBlock>())
                {
                    PlayerStats.Instance.isEnterEnd = true;
                    PlayerStats.Instance.CheckVictoryCondition();
                    //Debug.Log("找到EndBlock");
                }

                Die(); //Die有包括排头兵
                //GameObject.Destroy(soliderAgent.gameObject);
            }
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

            var nextTarget = nextBlock[0];
            var nextPoint = nextTarget.transform.position ;

            moveDir= nextPoint - (soliderAgent.transform.position- new Vector3(0f, soliderAgent.transform.localScale.y, 0f));
            //Vector3 dir = nextBlock[0].transform.position - soliderAgent.transform.position;
            RaycastHit hit;
            Vector3 startPoint = soliderAgent.transform.position;
            Debug.DrawRay(startPoint, moveDir.normalized * frontCheckDistance, Color.red);

            if (Physics.Raycast(startPoint, moveDir.normalized, out hit, frontCheckDistance))
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
                    // 不能被阻挡
                    return false;
                }
            }

            return false;
        }

        #endregion


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
        protected virtual bool HasAttackTarget()
        {
            // 绘制攻击判定范围的可视化效果
            DrawAttackRange();
            return false;
        }


        private void ClearTarget()
        {
            attackTargets.Clear();
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
            if (soliderModel.attackInterval < 0)
            {
                return;
            }


            await Task.Delay(TimeSpan.FromSeconds(soliderModel.attackInterval));
            isAttackReady = true;
        }

        #endregion


        //召唤
        public virtual void Summon()
        {
        }

        #region 受击

        public void OnTakeDamage(EnemyAgent enemyAgent)
        {
            AddAttacker(enemyAgent);

            var damagePoint = soliderAgent.buffManager.CalculateDamage(new DamageInfo(enemyAgent, soliderAgent));
            if (damagePoint == 0)
            {
                Debug.Log("士兵免伤，目前的血量是：" + soliderAgent.curHp);
                return;
            }

            Debug.Log("士兵扣血，目前的血量是：" + soliderAgent.curHp);
            soliderAgent.curHp -= damagePoint;
            if (soliderAgent)
            {
                soliderAgent.StartCoroutine(FlashRed());
            }

            if (soliderAgent.curHp <= 0)
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

        //受到AOE伤害后,根据当前的攻击者(敌人)的AOE攻击范围,以自己为中心寻找范围内的士兵,并使其造成伤害
        public void OnTakeAOEDamage(EnemyAgent enemyAgent, float aoeRange)
        {
            if (!soliderAgent)
            {
                return;
            }

            // 当前敌人受到伤害
            OnTakeDamage(enemyAgent);

            // 获取 aoeRange 范围内的所有敌人
            List<SoliderAgent> aoeTargets = GetAOETargets(soliderAgent.transform.position, aoeRange);
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

        #endregion

        #region 死亡

        protected virtual void Die()
        {
            Debug.Log($"{soliderModel.soliderName} has died!");

            foreach (var agent in attackers)
            {
                //通知在打他的敌人，他死了
                if (agent as EnemyAgent)
                {
                    var enemyAgent = agent.GetComponent<EnemyAgent>();
                    enemyAgent.enemyLogic.RemoveAttackTarget(soliderAgent);
                }

                //通知在辅助他的队友，他死了
                if (agent as SoliderAgent)
                {
                    var solider = agent.GetComponent<SoliderAgent>();
                    solider.soliderLogic.RemoveTarget(soliderAgent);
                }
            }


            //若该士兵是被阻挡的，通知被阻挡的人，他死了
            if (blocker != null)
            {
                blocker.enemyLogic.RemoveBlockTargets(soliderAgent);
            }

            soliderAgent.StopAllCoroutines();
            Object.Destroy(soliderAgent.gameObject);
        }

        #endregion

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