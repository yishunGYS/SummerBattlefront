using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_RangeMoveSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_RangeMoveSO")]
    public class SoliderState_RangeMoveSO: SoliderStateSO
    {
        private float timer;
        private const float moveTime = 1.2f;

        public SoliderState_RangeMoveSO()
        {
            stateType = UnitStateType.Move;
        }
        public override void OnEnter()
        {
            timer = 0;
        }

        public override void OnUpdate()
        {

            
            //������ɫ��������
            if (soliderAgent.soliderLogic.CheckObstacle())
            {
                fsm.ChangeState(UnitStateType.Idle);
            }
            //����ͣͣ
            timer += Time.deltaTime;
            if (timer>moveTime)
            {
                fsm.ChangeState(UnitStateType.Idle);
            }
            //�߼��л�
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
            timer = 0;
        }
    }
}