using Gameplay.Enemy;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_AttackSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_AttackSO")]
    public class SoliderState_AttackSO : SoliderStateSO
    {
        

        public SoliderState_AttackSO()
        {
            stateType = UnitStateType.Attack;
        }

        public override void OnEnter()
        {
            Debug.Log("½øÈë¹¥»÷×´Ì¬");
            //attackTimer = 0f; // ÖØÖÃ¼ÆÊ±Æ÷
            soliderAgent.soliderLogic.GetTarget();
        }

        public override void OnUpdate()
        {
            // attack to idle
            if (!soliderAgent.soliderLogic.HasAttackTarget())
            {
                fsm.ChangeState(UnitStateType.Idle);
            }
            
            soliderAgent.soliderLogic.Attack();
        }


        public override void OnFixedUpdate()
        {

        }

        public override void OnExit()
        {
            Debug.Log("ÍË³ö¹¥»÷×´Ì¬");
        }
    }
}
