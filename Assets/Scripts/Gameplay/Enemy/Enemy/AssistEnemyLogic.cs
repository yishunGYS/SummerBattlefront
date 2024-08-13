using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Enemy.Enemy
{
    public class AssistEnemyLogic : EnemyLogicBase
    {
        protected AssistEnemyLogic(EnemyAgent agent) : base(agent)
        {
        }

        protected override bool HasAttackTarget()
        {
            base.HasAttackTarget();
            Collider[] hitColliders =
                Physics.OverlapSphere(enemyAgent.transform.position, enemyModel.attackRange,
                    LayerMask.GetMask("Enemy"));

            int colliderCount = hitColliders.Length;
            foreach (var collider in hitColliders)
            {
                if (collider == enemyAgent.GetComponent<Collider>())
                {
                    colliderCount--;
                }
            }
            if (colliderCount<1)
            {
                return false;
            }

            return true;
        }

        protected void GetAssistTargetBaseDistance()
        {
            List<AttackEnemyTarget> tempDistanceTargets = new List<AttackEnemyTarget>();
        
            Collider[] hitColliders =
                Physics.OverlapSphere(enemyAgent.transform.position, enemyModel.attackRange,
                    LayerMask.GetMask("Enemy"));
            foreach (var collider in hitColliders)
            {
                var tempDis = Vector3.Distance(enemyAgent.transform.position, collider.transform.position);
                var temp = collider.GetComponent<EnemyAgent>();
                if (temp == enemyAgent)
                {
                    continue;
                }
                var tempTarget = new AttackEnemyTarget(tempDis, temp);
                tempDistanceTargets.Add(tempTarget);
            }
        
            SortMultiTargetsByDistance(tempDistanceTargets);
            for (int i = 0; i < enemyModel.attackNum; i++)
            {
                attackTargets.Add(tempDistanceTargets[i].target);
            }
        }
        
        private void SortMultiTargetsByDistance(List<AttackEnemyTarget> distanceTargets)
        {
            distanceTargets.Sort((a, b) => a.dis.CompareTo(b.dis));
        }


        public override void RemoveAttackTarget(UnitAgent target)
        {
            EnemyAgent enemyAgent = target as EnemyAgent;
            if (attackTargets.Contains(enemyAgent) && enemyAgent != null)
            {
                attackTargets.Remove(enemyAgent);
                Debug.Log($"Target removed: {enemyAgent.enemyModel.enemyName}");
            }
            
        }
    }
}
