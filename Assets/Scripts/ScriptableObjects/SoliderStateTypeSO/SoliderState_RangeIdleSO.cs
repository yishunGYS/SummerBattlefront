using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_RangeIdleSO",
        menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_RangeIdleSO")]
    public class SoliderState_RangeIdleSO : SoliderState_IdleBaseSO
    {
        private bool canEnterMove;
        private float timer;
        public float waitTime;

        public bool isFirstSpawn = true;


        public override void OnEnter()
        {
            base.OnEnter();

        }

        public override void OnUpdate()
        {
            //idle to move
            //if (soliderAgent.soliderLogic.CheckObstacle()==false && soliderAgent.soliderLogic.HasAttackTarget()==false) 
            //{
            //    fsm.ChangeState(UnitStateType.Move);
            //}
            //应该是远程的attack逻辑
            if (isFirstSpawn)
            {
                fsm.ChangeState(UnitStateType.Move);
                isFirstSpawn = false;
            }

            
            if (soliderAgent.soliderLogic.CheckObstacle() == false ||
                soliderAgent.soliderLogic.CheckCanAttack() == false)
            {
                timer += Time.deltaTime;
                if (timer > waitTime)
                {
                    fsm.ChangeState(UnitStateType.Move);
                    
                }
            }

            //idle to attack
            if (soliderAgent.soliderLogic.CheckCanAttack())
            {
                fsm.ChangeState(UnitStateType.Attack);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            timer = 0;
        }
    }
}