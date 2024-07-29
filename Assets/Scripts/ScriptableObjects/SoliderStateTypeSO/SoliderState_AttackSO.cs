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
            Debug.Log("进入攻击状态");
            //attackTimer = 0f; // 重置计时器
            soliderAgent.soliderLogic.GetTarget();
        }

        public override void OnUpdate()
        {
            // attack to idle
            if (!soliderAgent.soliderLogic.HasAttackTarget())
            {
                fsm.ChangeState(UnitStateType.Idle);
            }
            //正在攻击的敌人死了，也切回idle（放置远程兵在范围内是有目标的，但依然罚站
            
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
