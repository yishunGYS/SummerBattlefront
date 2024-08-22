using Gameplay.Enemy;
using UnityEngine;

namespace Gameplay.Features.EnemyFeature
{
    [RequireComponent(typeof(EnemyAgent))]
    public class UntargetableFeature : MonoBehaviour
    {
        private EnemyAgent agent;

        //���ɹ�����ǩ
        private string untargetableTag = "Untargetable";

        private void Awake()
        {
            agent = GetComponent<EnemyAgent>();
        }

        private void Start()
        {
            MakeUntargetable();
        }

        // �����˵�tag����Ϊ���ɹ���
        private void MakeUntargetable()
        {
            gameObject.tag = untargetableTag;
            Debug.Log($"{agent.enemyModel.enemyName} ������Ϊ���ɹ�����tag�޸�Ϊ {untargetableTag}");
            agent.enemyModel.blockNum = 0;
        }
    }
}
