using System.Collections.Generic;
using Gameplay.Enemy;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Features.EnemyFeature
{
    public class DistanceBasedFocusTargeting: EnemyGetTargetFeatureBase
    {
        public override void GetTarget()
        {
            base.GetTarget();
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

                if (m_enemyLogic.attackTargets.Contains(temp))
                {
                    continue;
                }

                var tempTarget = new AttackSoliderTarget(tempDis, temp);
                tempDistanceTargets.Add(tempTarget);
            }

            SortMultiTargetsByDistance(tempDistanceTargets);
            int targetsToAdd = CalculateRemainTargetToAdd(tempDistanceTargets.Count);
            for (int i = 0; i < targetsToAdd; i++)
            {
                m_enemyLogic.attackTargets.Add(tempDistanceTargets[i].target);
            }
        }
        
        private void SortMultiTargetsByDistance(List<AttackSoliderTarget> distanceTargets)
        {
            distanceTargets.Sort((a, b) => a.dis.CompareTo(b.dis));
        }
    }
    
}