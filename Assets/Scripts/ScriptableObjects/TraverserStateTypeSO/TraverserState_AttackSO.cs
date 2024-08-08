using ScriptableObjects.SoliderStateTypeSO;
using UnityEngine;

namespace ScriptableObjects.TraverserStateTypeSO
{
    [CreateAssetMenu(fileName = "TraverserState_AttackSO", menuName = "ScriptableObjects/TraverserState/TraverserState_AttackSO")]
    public class TraverserState_AttackSO : SoliderState_MeleeAttackSO
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnExit()
        {
            
        }
    }
}
