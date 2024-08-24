using Gameplay.Enemy;
using Gameplay.Enemy.Enemy;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Features.EnemyFeature
{
    [RequireComponent(typeof(EnemyAgent))]
    public class EnemyGetTargetFeatureBase : MonoBehaviour
    {
        protected EnemyAgent enemyAgent;
        protected EnemyLogicBase m_enemyLogic;
        protected EnemyModelBase enemyModel;
        

        public void OnInit()
        {
            enemyAgent = GetComponent<EnemyAgent>();
            m_enemyLogic = enemyAgent.enemyLogic;
            enemyModel = enemyAgent.enemyModel;
        }

        public virtual void GetTarget()
        {
            
        }
        
        protected void PrintCurrentTargets()
        {
            foreach (var target in m_enemyLogic.attackTargets)
            {
                Debug.Log($"当前目标: {target.name} HP为: {(target as SoliderAgent)?.curHp}");
            }
        }
        
         
        protected int CalculateRemainTargetToAdd(int potentialTargetsNum)
        {
            return Mathf.Min(enemyAgent.enemyModel.attackNum - m_enemyLogic.attackTargets.Count, potentialTargetsNum);
        }
        
        protected bool CheckMatchAttackType(SoliderAgent target)
        {
            if ((enemyModel.attackSoliderType & target.soliderModel.soliderType) == UnitType.None)
            {
                return false;
            }
            return true;
        }
        
        protected bool HasFocusTarget()
        {
            if (m_enemyLogic.attackTargets.Count >= enemyModel.attackNum)
                return true;
            return false;
        }
    }
}