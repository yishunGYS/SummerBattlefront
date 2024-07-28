using Gameplay.Enemy;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_AttackSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_AttackSO")]
    public class SoliderState_AttackSO : SoliderStateSO
    {
        private float attackTimer;

        public SoliderState_AttackSO()
        {
            stateType = UnitStateType.Attack;
        }

        public override void OnEnter()
        {
            Debug.Log("���빥��״̬");
            attackTimer = 0f; // ���ü�ʱ��
            soliderAgent.soliderLogic.GetTarget();
            foreach (var target in soliderAgent.soliderLogic.attackTargets)
            {
                Debug.Log(target.gameObject.name);
            }
        }

        public override void OnUpdate()
        {
            // �е���ʱ����
            if (soliderAgent.soliderLogic.attackTargets != null && soliderAgent.soliderLogic.attackTargets.Count > 0)
            {
                attackTimer += Time.deltaTime;

                if (attackTimer >= soliderAgent.soliderModel.attackInterval)
                {
                    // ����һ������������ö��
                    var targetsCopy = new List<EnemyAgent>(soliderAgent.soliderLogic.attackTargets);
                    foreach (var target in targetsCopy)
                    {
                        target.enemyLogic.OnTakeDamage(soliderAgent.soliderModel.attackPoint, soliderAgent.soliderModel.magicAttackPoint, soliderAgent);
                    }
                    attackTimer = 0f; // ���ü�ʱ��
                }
            }
            else
            {
                fsm.ChangeState(UnitStateType.Move);
            }
        }


        public override void OnFixedUpdate()
        {

        }

        public override void OnExit()
        {
            Debug.Log("�˳�����״̬");
        }
    }
}
