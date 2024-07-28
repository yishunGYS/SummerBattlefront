using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_MeleeIdleSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_MeleeIdleSO")]
    public class SoliderState_MeleeIdleSO : SoliderState_IdleBaseSO
    {

        public override void OnEnter()
        {
            Debug.Log("SoliderStateIdle OnEnter");
        }

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
            
            //Ïàµ±ÓÚUpdate StopÂß¼­
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
}
