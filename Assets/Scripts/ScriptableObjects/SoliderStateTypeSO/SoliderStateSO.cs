using Gameplay.Player;
using UnityEngine;
using Utilities;

namespace ScriptableObjects.SoliderStateTypeSO
{
    public abstract class SoliderStateSO : UnitStateSO
    {
        protected SoliderAgent soliderAgent;
        public override void OnLogin(StateMachine stateMachine, IAgent iAgent)
        {
            base.OnLogin(stateMachine, iAgent);
            soliderAgent = (SoliderAgent)agent;
        }
    }
}
