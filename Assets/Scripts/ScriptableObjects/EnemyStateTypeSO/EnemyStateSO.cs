using Gameplay.Enemy;
using Gameplay.Player;
using UnityEngine;
using Utilities;

namespace ScriptableObjects.EnemyStateTypeSO
{
    public abstract class EnemyStateSO : UnitStateSO
    {
        protected EnemyAgent enemyAgent;
        public override void OnLogin(StateMachine stateMachine, UnitAgent iAgent)
        {
            base.OnLogin(stateMachine, iAgent);
            enemyAgent = (EnemyAgent)agent;
        }
    }
}
