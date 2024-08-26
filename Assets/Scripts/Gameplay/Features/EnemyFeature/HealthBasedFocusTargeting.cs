using Gameplay.Enemy;
using Gameplay.Player;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Gameplay.Features.EnemyFeature
{
    [RequireComponent(typeof(EnemyAgent))]
    public class HealthBasedFocusTargeting : EnemyGetTargetFeatureBase
    {

        public override void GetTarget()
        {
            base.GetTarget();
            // if (HasFocusTarget())
            //     return;
            m_enemyLogic.attackTargets.Clear();
            List<SoliderAgent> potentialTargets = new List<SoliderAgent>();
            Collider[] hitColliders = Physics.OverlapSphere(enemyAgent.transform.position, enemyAgent.enemyModel.attackRange, LayerMask.GetMask("Solider"));

            foreach (var collider in hitColliders)
            {
                var solider = collider.GetComponent<SoliderAgent>();
                if (!CheckMatchAttackType(solider))
                {
                    continue;
                }
                if (solider == null || m_enemyLogic.attackTargets.Contains(solider))
                {
                    continue;
                }

                potentialTargets.Add(solider);
            }

            potentialTargets.Sort((a, b) => a.GetMaxHp().CompareTo(b.GetMaxHp()));

            int targetsToAdd = CalculateRemainTargetToAdd(potentialTargets.Count);
            for (int i = 0; i < targetsToAdd; i++)
            {
                m_enemyLogic.attackTargets.Add(potentialTargets[i]);
            }

            PrintCurrentTargets();
        }

        


    }
}
