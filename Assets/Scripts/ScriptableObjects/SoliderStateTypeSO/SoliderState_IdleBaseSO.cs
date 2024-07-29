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
            //¶¯»­
            Debug.Log("½øÈëIdle×´Ì¬");
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