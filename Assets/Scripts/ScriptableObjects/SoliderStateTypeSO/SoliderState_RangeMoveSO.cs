using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_RangeMoveSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_RangeMoveSO")]
    public class SoliderState_RangeMoveSO: SoliderStateSO
    {
        private const float enterToMoveTime = 0.2f;
        //private bool canMove = false;
        
       
        public SoliderState_RangeMoveSO()
        {
            stateType = UnitStateType.Move;
        }
        public override async void OnEnter()
        {
            Debug.Log("�����ƶ�״̬");
            // await Delay(enterToMoveTime);
            // canMove = true;
        }

        public override async void OnUpdate()
        {
            Debug.Log("�ƶ�״̬ update");
            //������ɫ��������
            if (soliderAgent.soliderLogic.CheckObstacle())
            {
                fsm.ChangeState(UnitStateType.Idle);
            }

            //�߼��л�
            if (soliderAgent.soliderLogic.CheckCanAttack())
            {
                fsm.ChangeState(UnitStateType.Attack);
            }

            // if (canMove)
            // {
            //     soliderAgent.soliderLogic.Move();
            // }
            soliderAgent.soliderLogic.Move();
            
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnExit()
        {
            //canMove = false;
        }

        private async Task Delay(float time)
        {
            await Task.Delay(TimeSpan.FromSeconds(time));
        }


    }
}