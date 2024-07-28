using Gameplay.Player;
using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_MoveSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_MoveSO")]
    public class SoliderState_MoveSO: SoliderStateSO
    {

        public SoliderState_MoveSO()
        {
            stateType = UnitStateType.Move;
        }
        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            //逻辑切换
            if (soliderAgent.soliderLogic.CheckCanAttack())
            {
                fsm.ChangeState(UnitStateType.Attack);
            }

            //辅助角色？？？？
            if (soliderAgent.soliderLogic.CheckObstacle())
            {
                fsm.ChangeState(UnitStateType.Idle);
            }
            
            soliderAgent.soliderLogic.Move();
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnExit()
        {
        
        }
    }
}