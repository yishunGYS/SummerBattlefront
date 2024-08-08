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

    public class StateMachine : MonoBehaviour
    {
        protected UnitAgent agent;
        [ShowInInspector]
        protected UnitStateSO curState;
        [SerializeField]
        protected SerializableDictionary<UnitStateType, UnitStateSO> stateDicts = new SerializableDictionary<UnitStateType, UnitStateSO>();

        protected Dictionary<UnitStateType, UnitStateSO> runtimeStateDict = new Dictionary<UnitStateType, UnitStateSO>();

        private void Awake()
        {
            agent = GetComponent<UnitAgent>();
        }

        public void OnInit()
        {
            CopeStateSo();
            foreach (var item in runtimeStateDict.Values)
            {
                if (item == null)
                {
                    Debug.LogError("StateSO is null in runtimeStateDict.");
                    continue;
                }
                item.OnLogin(this, agent);
            }
            
            curState = runtimeStateDict[UnitStateType.Idle];
            if (curState == null)
            {
                Debug.LogError("curState is null after initialization.");
            }
        }

        public virtual void CopeStateSo()
        {

        }

        public void ChangeState(UnitStateType stateType)
        {
            if (stateType == curState.stateType)
            {
                return;
            }
            curState.OnExit();
            curState = runtimeStateDict[stateType];
            if (curState == null)
            {
                Debug.LogError($"StateSO for {stateType} is null.");
                return;
            }
            Debug.Log("切换状态"+ curState);
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
