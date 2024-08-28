using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_RangeIdleSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_RangeIdleSO")]
    public class SoliderState_RangeIdleSO: SoliderState_IdleBaseSO
    {
        private bool canEnterMove;
        private float timer;
        public float waitTime;
        

        public override void OnUpdate()
        {
            //idle to move
            //if (soliderAgent.soliderLogic.CheckObstacle()==false && soliderAgent.soliderLogic.HasAttackTarget()==false) 
            //{
            //    fsm.ChangeState(UnitStateType.Move);
            //}
            //应该是远程的attack逻辑
            if (soliderAgent.soliderLogic.CheckObstacle() == false || soliderAgent.soliderLogic.CheckCanAttack() == false)
            {
                timer += Time.deltaTime;
                if (timer> waitTime)
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