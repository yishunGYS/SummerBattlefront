using System;
using System.Collections.Generic;
using Gameplay.Player;
using ScriptableObjects;
using UnityEngine;

namespace Utilities
{
    public enum UnitStateType
    {
        Idle,
        Attack,
        Move,
        Slow,
        Die
    }


    public class StateMachine: MonoBehaviour
    {
        protected IAgent agent;
        protected UnitStateSO curState;

        public List<UnitStateSO> stateList = new List<UnitStateSO>();


        private void Awake()
        {
            agent = GetComponent<IAgent>();
        }

        public void OnInit()
        {
            foreach (var item in stateList)
            {
                item.OnLogin(this,agent);
            }

            curState = stateList[0];
        }

        // protected void AddState(UnitState state)
        // {
        //     //stateDict[state.stateType] = state;
        //     state.OnLogin(this,agent);
        // }

        public void ChangeState(UnitStateType stateType)
        {
            if (stateType == curState.stateType)
            {
                return;
            }
            curState.OnExit();
            curState = stateList[(int)stateType];
            curState.OnEnter();
        }

        public void OnUpdate()
        {
            curState.OnUpdate();
        }
        
        public void OnFixedUpdate()
        {
            curState.OnFixedUpdate();
        }

        public UnitStateSO GetCurState()
        {
            return curState;
        }
    }
}
