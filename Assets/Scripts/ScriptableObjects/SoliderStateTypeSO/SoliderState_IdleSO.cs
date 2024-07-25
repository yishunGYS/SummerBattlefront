using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_IdleSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_IdleSO")]
    public class SoliderState_IdleSO : SoliderStateSO
    {
        public SoliderState_IdleSO()
        {
            stateType = UnitStateType.Idle;
        }

        public override void OnEnter()
        {
            Debug.Log("SoliderStateIdle OnEnter");
        }

        public override void OnUpdate()
        {
            Debug.Log($"{soliderAgent.soliderModel.soliderName}SoliderStateIdle OnUpdate");
            //idle to move
            if (soliderAgent.soliderLogic.CheckObstacle()==false && soliderAgent.soliderLogic.CheckCanAttack()==false)
            {
                fsm.ChangeState(UnitStateType.Move);
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
