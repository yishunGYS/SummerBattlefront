using Gameplay.Enemy;
using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Features.EnemyFeature
{
    [RequireComponent(typeof(EnemyAgent))]
    public class MoveDebuff : MonoBehaviour
    {
        private EnemyAgent agent;

        [Header("减少的百分比")]
        [SerializeField] private float decreasePercentage = 0.5f;

        [Header("减速持续时间")]
        [SerializeField] private float debuffDuration = 0.2f;

        // 用于跟踪所有被减速的士兵及其原始速度
        private Dictionary<SoliderAgent, float> debuffedSoldiers = new Dictionary<SoliderAgent, float>();

        private void Awake()
        {
            agent = GetComponent<EnemyAgent>();
        }

        private void Update()
        {
            if (agent.enemyLogic.isAttackReady && agent.enemyLogic.attackTargets.Count > 0)
            {
                ApplyDebuffToAllTargets();
            }
        }

        private void ApplyDebuffToAllTargets()
        {
            foreach (UnitAgent target in agent.enemyLogic.attackTargets)
            {
                if (target is SoliderAgent solider && !debuffedSoldiers.ContainsKey(solider))
                {
                    debuffedSoldiers[solider] = solider.soliderModel.moveSpeed;
                    StartCoroutine(ApplyDebuff(solider));
                }
            }
        }

        private IEnumerator ApplyDebuff(SoliderAgent target)
        {
            if (target == null)
                yield break;

            target.soliderModel.moveSpeed *= (1f - decreasePercentage);

            yield return new WaitForSeconds(debuffDuration);

            if (target != null && debuffedSoldiers.ContainsKey(target))
            {
                target.soliderModel.moveSpeed = debuffedSoldiers[target];
                debuffedSoldiers.Remove(target);
            }
        }

        private void OnDestroy()
        {
            foreach (var pair in debuffedSoldiers)
            {
                if (pair.Key != null)
                {
                    pair.Key.soliderModel.moveSpeed = pair.Value;
                }
            }
            debuffedSoldiers.Clear();
        }
    }
}
