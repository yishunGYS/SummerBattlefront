using Gameplay.Player;
using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "HeadSoliderState_IdleBaseSO", menuName = "ScriptableObjects/HeadSoliderStateTypeSO/HeadSoliderState_IdleBaseSO")]
    public class HeadSoliderState_IdleBaseSO : SoliderState_IdleBaseSO
    {
        public override void OnEnter()
        {
            Debug.Log("SoliderStateIdle OnEnter");
        }

        public override void OnUpdate()
        {
            //idle to move
            if (soliderAgent.soliderLogic.CheckObstacle() == false)
            {
                fsm.ChangeState(UnitStateType.Move);
            }
            //idle to attack
            //if (soliderAgent.soliderLogic.CheckCanAttack())
            //{
            //    fsm.ChangeState(UnitStateType.Attack);
            //}

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
