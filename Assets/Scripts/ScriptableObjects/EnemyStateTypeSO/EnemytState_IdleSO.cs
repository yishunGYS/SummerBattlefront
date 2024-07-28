using Gameplay.Player;
using UnityEngine;
using Utilities;

namespace ScriptableObjects.EnemyStateTypeSO
{
    [CreateAssetMenu(fileName = "EnemytState_IdleSO", menuName = "ScriptableObjects/EnemyStateTypeSO/EnemytState_IdleSO")]
    public class EnemytState_IdleSO : EnemyStateSO
    {
        public EnemytState_IdleSO()
        {
            stateType = UnitStateType.Idle;
        }

        public override void OnEnter()
        {
            Debug.Log("EnemytStateIdle OnEnter");
        }

        public override void OnUpdate()
        {
            Debug.Log("1");
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnExit()
        {

        }
    }
}
