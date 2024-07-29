using Gameplay.Enemy;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_MeleeAttackSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_MeleeAttackSO")]
    public class SoliderState_MeleeAttackSO : SoliderStateSO
    {
        

        public SoliderState_MeleeAttackSO()
        {
            stateType = UnitStateType.Attack;
        }

        public override void OnEnter()
        {
            Debug.Log("���빥��״̬");
            soliderAgent.soliderLogic.GetTarget();
        }

        public override void OnUpdate()
        {
            
            // attack to idle
            if (!soliderAgent.soliderLogic.CheckCanAttack())
            {
                fsm.ChangeState(UnitStateType.Idle);
            }
            Debug.Log("����״̬ update");
            
            soliderAgent.soliderLogic.Attack();
        }


        public override void OnFixedUpdate()
        {

        }

        public override void OnExit()
        {
            Debug.Log("�˳�����״̬");
        }
    }
}
