using UnityEngine;
using Utilities;

namespace ScriptableObjects.EnemyStateTypeSO
{
    [CreateAssetMenu(fileName = "EnemyState_AttackSO", menuName = "ScriptableObjects/EnemyStateTypeSO/EnemyState_AttackSO")]
    public class EnemyState_AttackSO : EnemyStateSO
    {
        public EnemyState_AttackSO()
        {
            stateType = UnitStateType.Attack;
        }

        public override void OnEnter()
        {
            Debug.Log("EnemytStateIdle Attack");
        }

        public override void OnUpdate()
        {

        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnExit()
        {

        }
    }
}
