using Gameplay.Player;
using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_MeleeMoveSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_MeleeMoveSO")]
    public class SoliderState_MeleeMoveSO: SoliderStateSO
    {

        public SoliderState_MeleeMoveSO()
        {
            stateType = UnitStateType.Move;
        }
        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            //辅助角色？？？？
            if (soliderAgent.soliderLogic.CheckObstacle())
            {
                fsm.ChangeState(UnitStateType.Idle);
            }
            
            //逻辑切换
            if (soliderAgent.soliderLogic.CheckCanAttack())
            {
                fsm.ChangeState(UnitStateType.Attack);
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