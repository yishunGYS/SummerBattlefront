using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    public class SoliderState_RangeIdleSO: SoliderState_IdleBaseSO
    {
        public override void OnUpdate()
        {
            //idle to move
            if ((soliderAgent.soliderLogic.CheckObstacle()==false && soliderAgent.soliderLogic.HasAttackTarget()==false)||
                 (soliderAgent.soliderLogic.CheckObstacle()==false && soliderAgent.soliderLogic.isAttackReady == false )) //�ڹ���cdҲ����
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