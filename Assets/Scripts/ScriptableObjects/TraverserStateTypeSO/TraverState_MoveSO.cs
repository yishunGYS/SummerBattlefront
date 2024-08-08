using Gameplay.Player.Solider.Attacker.Traverser;
using ScriptableObjects.SoliderStateTypeSO;
using UnityEngine;
using Utilities;

namespace ScriptableObjects.TraverserStateTypeSO
{
    [CreateAssetMenu(fileName = "TraverState_MoveSO", menuName = "ScriptableObjects/TraverserState/TraverState_MoveSO")]
    public class TraverState_MoveSO : SoliderStateSO
    {
        public TraverState_MoveSO()
        {
            stateType = UnitStateType.Move;
        }

        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            TraverserFeature feature = soliderAgent.GetComponent<TraverserFeature>();
            if (soliderAgent.soliderLogic.CheckObstacle()) //当前角色被阻挡
            {
                if (feature.canTraver) //当前第一次被阻挡,进入穿越状态
                {
                    TriggerTraver(feature);
                }
                else if (feature.isTraver && soliderAgent.soliderLogic.blocker != feature.preEnemy) //当前处于穿越状态且再次碰到的阻挡者不为上一个
                {
                    EndTraver(feature);
                }
                else //以上都不成立,说明穿越结束了,被正常阻挡/进行攻击
                {
                    fsm.ChangeState(UnitStateType.Idle);
                }
            }
            //当前角色未被阻挡,检测是否到达目标格子,若到达目标格子则结束穿越过程
            if (CheckArriveTarget(feature))//如果到达目标格子
            {
                EndTraver(feature);
            }
        }
        public override void OnFixedUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }

        private bool CheckArriveTarget(TraverserFeature feature) //检查当前是否到达目标格子
        {
            return soliderAgent.soliderLogic.currentBlock == feature.targetCell;
        }
        private void TriggerTraver(TraverserFeature feature)//触发穿越
        {
            feature.targetCell = soliderAgent.soliderLogic.currentBlock.nextCells[0].nextCells[0];//临时方案

            feature.isTraver = true;
            feature.canTraver = false;
            //速度更新
            feature.preSpeed = soliderAgent.soliderModel.moveSpeed;
            soliderAgent.soliderModel.moveSpeed = feature.traverSpeed;
            //记录敌人
            feature.preEnemy = soliderAgent.soliderLogic.blocker;
        }
        private void EndTraver(TraverserFeature feature)//穿越结束
        {
            feature.isTraver = false;
            soliderAgent.soliderModel.moveSpeed = feature.preSpeed;
        }

    }
}
