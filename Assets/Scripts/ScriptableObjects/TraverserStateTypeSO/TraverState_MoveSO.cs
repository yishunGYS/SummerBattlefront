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
            if (soliderAgent.soliderLogic.CheckObstacle()) //��ǰ��ɫ���赲
            {
                if (feature.canTraver) //��ǰ��һ�α��赲,���봩Խ״̬
                {
                    TriggerTraver(feature);
                }
                else if (feature.isTraver && soliderAgent.soliderLogic.blocker != feature.preEnemy) //��ǰ���ڴ�Խ״̬���ٴ��������赲�߲�Ϊ��һ��
                {
                    EndTraver(feature);
                }
                else //���϶�������,˵����Խ������,�������赲/���й���
                {
                    fsm.ChangeState(UnitStateType.Idle);
                }
            }
            //��ǰ��ɫδ���赲,����Ƿ񵽴�Ŀ�����,������Ŀ������������Խ����
            if (CheckArriveTarget(feature))//�������Ŀ�����
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

        private bool CheckArriveTarget(TraverserFeature feature) //��鵱ǰ�Ƿ񵽴�Ŀ�����
        {
            return soliderAgent.soliderLogic.currentBlock == feature.targetCell;
        }
        private void TriggerTraver(TraverserFeature feature)//������Խ
        {
            feature.targetCell = soliderAgent.soliderLogic.currentBlock.nextCells[0].nextCells[0];//��ʱ����

            feature.isTraver = true;
            feature.canTraver = false;
            //�ٶȸ���
            feature.preSpeed = soliderAgent.soliderModel.moveSpeed;
            soliderAgent.soliderModel.moveSpeed = feature.traverSpeed;
            //��¼����
            feature.preEnemy = soliderAgent.soliderLogic.blocker;
        }
        private void EndTraver(TraverserFeature feature)//��Խ����
        {
            feature.isTraver = false;
            soliderAgent.soliderModel.moveSpeed = feature.preSpeed;
        }

    }
}
