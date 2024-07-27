using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_AttackSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_AttackSO")]
    public class SoliderState_AttackSO : SoliderStateSO
    {

        public SoliderState_AttackSO()
        {
            stateType = UnitStateType.Attack;
        }
        public override void OnEnter()
        {
            Debug.Log("½øÈë¹¥»÷×´Ì¬");
        }

        public override void OnUpdate()
        {

        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnExit()
        {

        }
    }
}