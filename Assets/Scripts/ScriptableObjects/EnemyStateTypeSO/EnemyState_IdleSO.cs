using Gameplay.Player;
using UnityEngine;
using Utilities;

namespace ScriptableObjects.EnemyStateTypeSO
{
    [CreateAssetMenu(fileName = "EnemyState_IdleSO", menuName = "ScriptableObjects/EnemyStateTypeSO/EnemyState_IdleSO")]
    public class EnemyState_IdleSO : EnemyStateSO
    {
        public EnemyState_IdleSO()
        {
            stateType = UnitStateType.Idle;
        }

        public override void OnEnter()
        {
            Debug.Log("EnemytStateIdle OnEnter");
        }

        public override void OnUpdate()
        {
            if (enemyAgent.enemyLogic.CheckCanAttack())
            {
                fsm.ChangeState(UnitStateType.Attack);
            }
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnExit()
        {

        }
    }
}
