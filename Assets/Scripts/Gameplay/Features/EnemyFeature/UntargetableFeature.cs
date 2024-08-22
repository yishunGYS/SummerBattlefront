using Gameplay.Enemy;
using UnityEngine;

namespace Gameplay.Features.EnemyFeature
{
    [RequireComponent(typeof(EnemyAgent))]
    public class UntargetableFeature : MonoBehaviour
    {
        private EnemyAgent agent;

        //不可攻击标签
        private string untargetableTag = "Untargetable";

        private void Awake()
        {
            agent = GetComponent<EnemyAgent>();
        }

        private void Start()
        {
            MakeUntargetable();
        }

        // 将敌人的tag设置为不可攻击
        private void MakeUntargetable()
        {
            gameObject.tag = untargetableTag;
            Debug.Log($"{agent.enemyModel.enemyName} 已设置为不可攻击，tag修改为 {untargetableTag}");
            agent.enemyModel.blockNum = 0;
        }
    }
}
