using UnityEngine;

namespace Gameplay.Enemy.Enemy.Archers
{
    public class ArchersLogic : AttackEnemyLogic
    {
        private Archers archerAgent;
        public ArchersLogic(EnemyAgent agent) : base(agent)
        {
            archerAgent = agent as Archers;
        }

        public override void GetTarget()
        {
            base.GetTarget();
            enemyGetTargetFeature.GetTarget();
        }

        public override void Attack()
        {
            base.Attack();
            if (isAttackReady)
            {
                CalculateCd();
                //·¢ÉäÍ¶ÖÀÎï
                for (int i = attackTargets.Count-1; i >= 0; i--)
                {
                    Debug.Log("Enemy¹¥»÷£¡£¡£¡");
                    var go =  GameObject.Instantiate(archerAgent.projectile, archerAgent.transform.position, Quaternion.identity);
                    go.OnInit(archerAgent, attackTargets[i]);
                }
            }
            //NormalAttack();
        }
    }
}


