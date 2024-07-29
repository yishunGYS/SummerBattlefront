using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_RangeIdleSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_RangeIdleSO")]
    public class SoliderState_RangeIdleSO: SoliderState_IdleBaseSO
    {
        public override void OnUpdate()
        {
            //idle to move
            if (soliderAgent.soliderLogic.CheckObstacle()==false && soliderAgent.soliderLogic.HasAttackTarget()==false) 
            {
                fsm.ChangeState(UnitStateType.Move);
            }

            
            //idle to attack
            if (soliderAgent.soliderLogic.CheckCanAttack())
            {
                fsm.ChangeState(UnitStateType.Attack);
            }
        }
    }
}