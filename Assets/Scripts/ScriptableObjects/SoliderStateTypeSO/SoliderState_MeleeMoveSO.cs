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
            //逻辑切换
            if (soliderAgent.soliderLogic.CheckCanAttack())//范围内存在敌人,且攻击CD结束,则进入攻击状态
            {
                fsm.ChangeState(UnitStateType.Attack);
            }

            //辅助角色？？？？
            if (soliderAgent.soliderLogic.CheckObstacle())//范围内存在敌人,但攻击还在CD中,则进入Idle状态,被阻挡
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