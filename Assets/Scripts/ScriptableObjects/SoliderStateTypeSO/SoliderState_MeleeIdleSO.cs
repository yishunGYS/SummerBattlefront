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
            if (soliderAgent.soliderLogic.CheckObstacle()==false)//范围内不存在障碍物(包括敌人),则进入移动状态
            {
                fsm.ChangeState(UnitStateType.Move);
            }
            //idle to attack
            if (soliderAgent.soliderLogic.CheckCanAttack())
            {
                fsm.ChangeState(UnitStateType.Attack);
            }
            
            //相当于Update Stop逻辑
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
}
