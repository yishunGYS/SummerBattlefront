using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    public class SoliderState_IdleBaseSO: SoliderStateSO
    {
        public SoliderState_IdleBaseSO()
        {
            stateType = UnitStateType.Idle;
        }
        public override void OnEnter()
        {
            //����
            Debug.Log("����Idle״̬");
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