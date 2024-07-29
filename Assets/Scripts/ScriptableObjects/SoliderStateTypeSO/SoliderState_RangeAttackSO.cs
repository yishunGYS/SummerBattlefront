using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_RangeAttackSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_RangeAttackSO")]
    public class SoliderState_RangeAttackSO: SoliderStateSO
    {

        
        
        public SoliderState_RangeAttackSO()
        {
            stateType = UnitStateType.Attack;
        }
        public override void OnEnter()
        {
            Debug.Log("进入攻击状态");
            soliderAgent.soliderLogic.GetTarget();
        }

        public override void OnUpdate()
        {
            // attack to idle
            if (!soliderAgent.soliderLogic.CheckCanAttack())
            {
                fsm.ChangeState(UnitStateType.Idle);
            }
            
            //应该是远程的attack逻辑
            // if (soliderAgent.soliderLogic.CheckObstacle() == false && soliderAgent.soliderLogic.isAttackReady == false)
            // {
            //     fsm.ChangeState(UnitStateType.Move);
            // }
            soliderAgent.soliderLogic.Attack();
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnExit()
        {
            Debug.Log("退出攻击状态");
        }
    }
}