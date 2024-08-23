using Gameplay.Enemy;
using Gameplay.Player;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Gameplay.Features.EnemyFeature
{
    [RequireComponent(typeof(EnemyAgent))]
    public class HealthBasedFocusTargeting : MonoBehaviour
    {
        private EnemyAgent enemyAgent;
        private EnemyLogicBase enemyLogic;
        private MethodInfo originalGetFocusTargetMethod;

        private void Start()
        {
            enemyAgent = GetComponent<EnemyAgent>();
            enemyLogic = enemyAgent.enemyLogic;

            originalGetFocusTargetMethod = enemyLogic.GetType().GetMethod("GetFocusTarget", BindingFlags.NonPublic | BindingFlags.Instance);

            if (originalGetFocusTargetMethod != null)
            {
                ReplaceGetFocusTarget();
            }
            else
            {
                Debug.LogWarning("没找到GetFocusTarget方法");
            }
        }

        private void Update()
        {
            ReplaceGetFocusTarget();
        }

        private void ReplaceGetFocusTarget()
        {
            GetFocusTargetByHealth();
        }

        private void GetFocusTargetByHealth()
        {
            //Debug.Log("执行血量最低的FocusAttack!!!!!!!!!!!!!!!!");

            if (enemyLogic.attackTargets.Count >= enemyAgent.enemyModel.attackNum)
                return;

            List<SoliderAgent> potentialTargets = new List<SoliderAgent>();
            Collider[] hitColliders = Physics.OverlapSphere(enemyAgent.transform.position, enemyAgent.enemyModel.attackRange, LayerMask.GetMask("Solider"));

            foreach (var collider in hitColliders)
            {
                var solider = collider.GetComponent<SoliderAgent>();
                if (solider == null || enemyLogic.attackTargets.Contains(solider))
                {
                    continue;
                }

                potentialTargets.Add(solider);
            }

            potentialTargets.Sort((a, b) => a.curHp.CompareTo(b.curHp));

            int targetsToAdd = Mathf.Min(enemyAgent.enemyModel.attackNum - enemyLogic.attackTargets.Count, potentialTargets.Count);
            for (int i = 0; i < targetsToAdd; i++)
            {
                enemyLogic.attackTargets.Add(potentialTargets[i]);
            }

            PrintCurrentTargets();
        }

        private void PrintCurrentTargets()
        {
            foreach (var target in enemyLogic.attackTargets)
            {
                Debug.Log($"当前目标: {target.name} HP为: {(target as SoliderAgent)?.curHp}");
            }
        }
    }
}
