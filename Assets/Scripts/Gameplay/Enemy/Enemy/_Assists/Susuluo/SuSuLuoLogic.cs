using Gameplay.Features.EnemyFeature;
using UnityEngine;

namespace Gameplay.Enemy.Enemy._Assists.Susuluo
{
    public class SuSuLuoLogic : AssistEnemyLogic
    {
        public SuSuLuoLogic(EnemyAgent agent) : base(agent)
        {
        }

        public override void GetTarget()
        {
            base.GetTarget();
            GetAssistTargetBaseDistance();
        }

        public override void Attack()
        {
            base.Attack();
            if (isAttackReady)
            {
                CalculateCd();
                enemyAgent.GetComponent<EnemyCureFeature>().Cure();
            }
        }
    }
}
