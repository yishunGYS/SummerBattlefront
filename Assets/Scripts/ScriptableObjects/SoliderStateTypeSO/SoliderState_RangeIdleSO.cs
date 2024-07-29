using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_RangeIdleSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_RangeIdleSO")]
    public class SoliderState_RangeIdleSO: SoliderState_IdleBaseSO
    {
        private bool canEnterMove;
        private float timer;


        public override void OnUpdate()
        {
            //idle to move
            if (soliderAgent.soliderLogic.CheckObstacle()==false && soliderAgent.soliderLogic.HasAttackTarget()==false) 
            {
                fsm.ChangeState(UnitStateType.Move);
            }
            //应该是远程的attack逻辑
            if (soliderAgent.soliderLogic.CheckObstacle() == false && soliderAgent.soliderLogic.isAttackReady == false)
            {
                timer += Time.deltaTime;
                if (timer> 0.2f)
                {
                    fsm.ChangeState(UnitStateType.Move);
                    timer = 0;
                }
                
            }
            
            //idle to attack
            if (soliderAgent.soliderLogic.CheckCanAttack())
            {
                fsm.ChangeState(UnitStateType.Attack);
            }
        }
        
        
    }
}