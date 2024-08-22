using Gameplay.Enemy;
using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Features.EnemyFeature
{
    [RequireComponent(typeof(EnemyAgent))]
    public class FreezeDebuff : MonoBehaviour
    {
        private EnemyAgent agent;

        [Header("�������ʱ��")]
        [SerializeField] private float rootDuration = 1.0f;

        [Header("������")]
        [SerializeField] private float rootChance = 0.3f;

        private Dictionary<SoliderAgent, float> rootedSoldiers = new Dictionary<SoliderAgent, float>();

        private void Awake()
        {
            agent = GetComponent<EnemyAgent>();
        }

        private void Update()
        {
            if (agent.enemyLogic.isAttackReady && agent.enemyLogic.attackTargets.Count > 0)
            {
                ApplyRootDebuffToAllTargets();
            }
        }

        private void ApplyRootDebuffToAllTargets()
        {
            foreach (UnitAgent target in agent.enemyLogic.attackTargets)
            {
                if (target is SoliderAgent solider && !rootedSoldiers.ContainsKey(solider))
                {
                    // ��30%�ļ��ʶ���Ŀ��
                    if (Random.value < rootChance)
                    {
                        rootedSoldiers[solider] = solider.soliderModel.moveSpeed;
                        StartCoroutine(ApplyRootDebuff(solider));
                    }
                }
            }
        }

        private IEnumerator ApplyRootDebuff(SoliderAgent target)
        {
            if (target == null)
                yield break;

            target.soliderModel.moveSpeed = 0f;

            yield return new WaitForSeconds(rootDuration);

            if (target != null && rootedSoldiers.ContainsKey(target))
            {
                target.soliderModel.moveSpeed = rootedSoldiers[target];
                rootedSoldiers.Remove(target);
            }
        }

        private void OnDestroy()
        {
            foreach (var pair in rootedSoldiers)
            {
                if (pair.Key != null)
                {
                    pair.Key.soliderModel.moveSpeed = pair.Value;
                }
            }
            rootedSoldiers.Clear();
        }
    }
}
