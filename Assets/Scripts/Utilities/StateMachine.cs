using System;
using System.Collections.Generic;
using Gameplay.Player;
using ScriptableObjects;
using ScriptableObjects.SoliderStateTypeSO;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace Utilities
{
    public enum UnitStateType
    {
        Idle,
        Move,
        Attack,
        Slow,
        Die
    }


    public class StateMachine: MonoBehaviour
    {
        protected IAgent agent;
        protected UnitStateSO curState;
        [SerializeField]private SerializableDictionary<UnitStateType, UnitStateSO> stateDicts = new SerializableDictionary<UnitStateType, UnitStateSO>();

        private Dictionary<UnitStateType, UnitStateSO> runtimeStateDict = new Dictionary<UnitStateType, UnitStateSO>();
        private void Awake()
        {
            agent = GetComponent<IAgent>();
        }

        public void OnInit()
        {
            CopeStateSo();
            foreach (var item in runtimeStateDict.Values)
            {
                item.OnLogin(this,agent);
            }

            //确保一定有idle状态
            if (!runtimeStateDict.ContainsKey(UnitStateType.Idle))
            {
                runtimeStateDict.Add(UnitStateType.Idle,ScriptableObject.CreateInstance<SoliderState_IdleSO>());
            }
            
            curState = runtimeStateDict[UnitStateType.Idle];
        }

        private void CopeStateSo()
        {
            
            foreach (var item in stateDicts.Values)
            {
                var tempSo = StateSoIndustry.CreateStateSo(item.stateType);
                runtimeStateDict.Add(item.stateType,tempSo);
            }
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
            curState = runtimeStateDict[stateType];
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
    
    public class StateSoIndustry
    {
        public static UnitStateSO CreateStateSo(UnitStateType stateType)
        {
            if (stateType == UnitStateType.Idle)
            {
                return ScriptableObject.CreateInstance<SoliderState_IdleSO>();
            }
            else if (stateType == UnitStateType.Move)
            {
                return ScriptableObject.CreateInstance<SoliderState_MoveSO>();
            }

            return null;
        }
    }
}
