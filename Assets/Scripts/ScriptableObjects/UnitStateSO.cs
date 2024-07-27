using Gameplay.Player;
using UnityEngine;
using Utilities;

namespace ScriptableObjects
{
    public abstract class UnitStateSO : ScriptableObject
    {
        protected IAgent agent;
        protected StateMachine fsm;
        public UnitStateType stateType;

        public virtual void OnLogin(StateMachine stateMachine,IAgent iAgent)
        {
            fsm = stateMachine;
            agent = iAgent;
        }
        
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnFixedUpdate();
        public abstract void OnExit();
    }
}
