using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    [CreateAssetMenu(fileName = "SoliderState_MoveSO", menuName = "ScriptableObjects/SoliderStateTypeSO/SoliderState_MoveSO")]
    public class SoliderState_MoveSO: SoliderStateSO
    {

        public SoliderState_MoveSO()
        {
            stateType = UnitStateType.Move;
        }
        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            //移动逻辑
            Debug.Log($"{soliderAgent.soliderModel.soliderName}SoliderStateMove OnUpdate");
            soliderAgent.soliderLogic.Move();
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnExit()
        {
        
        }
    }
}